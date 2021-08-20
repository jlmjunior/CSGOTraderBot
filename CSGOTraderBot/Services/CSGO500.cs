using CSGOTraderBot.Api;
using CSGOTraderBot.Models;
using CSGOTraderBot.Models.CSGO500.Inventory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System;

namespace CSGOTraderBot.Services
{
    public class CSGO500
    {
        private readonly Get _get;
        private readonly Post _post;
        private readonly SteamTrade.Offer steamOffer;

        public CSGO500()
        {
            _get = new Get();
            _post = new Post();

            steamOffer = new SteamTrade.Offer(
                Helper.Config.Get("sessionid", "SteamSettings"), 
                Helper.Config.Get("steamLoginSecure", "SteamSettings"));
        }

        public async Task<ResultModel> ConfirmItems()
        {
            var result = new ResultModel()
            {
                Success = true,
                Message = new List<string>()
            };

            var resultJson = await GetInventory();

            if (resultJson == "unauthorized")
            {
                result.Success = false;
                result.Message.Add("Cookie inválido.");

                return result;
            }

            var inventory = JsonConvert.DeserializeObject<RootObject>(resultJson);

            if (inventory == null)
            {
                result.Success = true;
                //result.Message.Add("Nenhum item encontrado no inventário.");

                return result;
            }

            var confirmInventory = inventory.Listings
                .FindAll(i => i.Status == "4");

            if (confirmInventory.Count == 0)
            {
                result.Success = true;
                //result.Message.Add("Nenhum item encontrado para confirmação.");

                return result;
            }

            string csrfToken = string.Empty;

            try
            {
                csrfToken = await GetCsrfToken();
            }
            catch (Exception ex)
            {
                Helper.Log.SaveError(ex.ToString());

                result.Success = false;
                result.Message.Add("Falha ao obter CSRF Token.");

                return result;
            }

            if (string.IsNullOrWhiteSpace(csrfToken))
            {
                result.Success = false;
                result.Message.Add("Falha ao obter CSRF Token.");

                return result;
            }

            List<string> itemsToConfirm = new List<string>();

            confirmInventory.ForEach(i => 
            {
                result.Message
                .Add($"Enviado confirmação do item: {i.Name} || Status: {i.NiceStatus} || Valor: {i.Value} || Comprador ID: {i.RequestOpenId}");

                var discartResult = Task.Run(() => ConfirmItem(i.Id, csrfToken)).Result;

                i.Items.ToList()
                .ForEach(x => itemsToConfirm.Add(x.AssetId));
            });

            if (itemsToConfirm.Count > 0)
            {
                var resultConfirmSteamCsgo500 = Task.Run(() => ConfirmSteam(itemsToConfirm)).Result;

                result.Success = resultConfirmSteamCsgo500.Success;
                result.Message.AddRange(resultConfirmSteamCsgo500.Message);
            }

            return result;
        }

        public async Task<ResultModel> ConfirmSteam(List<string> itemsToConfirm)
        {
            var result = new ResultModel()
            {
                Success = true,
                Message = new List<string>()
            };

            var resultJson = await GetInventory();

            var inventory = JsonConvert.DeserializeObject<RootObject>(resultJson);

            if (inventory == null)
            {
                result.Success = true;
                result.Message.Add("Steam - Nenhum item encontrado no inventário.");

                return result;
            }

            var itemsWaitForTradeOffer = inventory.Listings
                .FindAll(i => i.Status == "6");

            if (itemsWaitForTradeOffer == null)
            {
                result.Success = true;
                result.Message.Add("Steam - Nenhum item encontrado aguardando envio.");

                return result;
            }


            foreach (var item in itemsWaitForTradeOffer)
            {
                Item itemWaitForTradeOffer = null;
                bool itemNotValid = false;

                try
                {
                    itemWaitForTradeOffer = item.Items.SingleOrDefault();

                    itemNotValid = (!itemsToConfirm.Contains(itemWaitForTradeOffer.AssetId)) && itemWaitForTradeOffer == null;
                }
                catch (Exception ex)
                {
                    Helper.Log.SaveError(ex.ToString());

                    result.Success = false;
                    result.Message.Add("Steam - Falha ao obter item para confirmação.");

                    return result;
                }

                if (itemNotValid)
                {
                    result.Success = false;
                    result.Message.Add("Steam - Falha ao obter item para confirmação.");

                    return result;
                }

                var offer = new SteamTrade.Models.OfferModel()
                {
                    TradeLink = item.RequestTradeURL,
                    Partner = item.RequestOpenId,
                    JsonOffer = new SteamTrade.Models.JsonTradeOfferModel()
                    {
                        MeOffer = new SteamTrade.Models.Me()
                        {
                            Assets = new SteamTrade.Models.Assets[]
                            {
                                new SteamTrade.Models.Assets()
                                {
                                    AssetId = itemWaitForTradeOffer.AssetId
                                }
                            }
                        }
                    }
                };

                var resultSteamConfirm = Task.Run(() => steamOffer.Send(offer)).Result;

                if (!resultSteamConfirm.Success)
                {
                    var steam = new Steam();
                    var resultLogin = Task.Run(() => steam.CheckLogin()).Result;

                    if (resultLogin.Success)
                    {
                        steamOffer.SetSteamLoginSecure(Helper.Config.Get("steamLoginSecure", "SteamSettings"));
                        resultSteamConfirm = Task.Run(() => steamOffer.Send(offer)).Result;
                    }   
                }

                if (resultSteamConfirm.Success)
                    result.Message.Add($"Steam - Oferta enviada para o perfil {item.RequestOpenId} do item: {itemWaitForTradeOffer.Name}");
                else
                {
                    result.Success = false;
                    result.Message.Add($"Steam - Falha ao enviar o item: {itemWaitForTradeOffer.Name} para o perfil {item.RequestOpenId}");
                    result.Message.AddRange(resultSteamConfirm.Messages);
                }
            }

            return result;
        }

        private async Task<string> GetInventory()
        {
            var response = await _get.Async("https://csgo500.com/marketplace/all/730",
                new List<Cookie>() { new Cookie("express.sid", Helper.Config.Get("express.sid", "CSGO500")) });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();
                return "unauthorized";
            }
                
            string result = await response.Content.ReadAsStringAsync();
            response.Dispose();

            return result;
        }

        private async Task<string> GetCsrfToken()
        {
            var response = await _get.Async("https://csgo500.com/",
                new List<Cookie>() { new Cookie("express.sid", Helper.Config.Get("express.sid", "CSGO500")) });

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();
                return null;
            }

            string result = await response.Content.ReadAsStringAsync();
            response.Dispose();

            int indexInit = result.IndexOf("csrfToken") + 13;
            int lenght = result.IndexOf("\"", indexInit) - indexInit;

            return result.Substring(indexInit, lenght);
        }

        private async Task<string> ConfirmItem(string itemId, string csrfToken)
        {
            return await _post.Async("https://csgo500.com/marketplace/confirm",
                new List<Cookie>() { new Cookie("express.sid", Helper.Config.Get("express.sid", "CSGO500")) },
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("listingId", itemId),
                    new KeyValuePair<string, string>("_csrf", csrfToken)
                });
        }
    }
}
