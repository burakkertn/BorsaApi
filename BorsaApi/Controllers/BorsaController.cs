using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BorsaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BorsaController : ControllerBase
    {

        private readonly HttpClient _httpClient;

        public BorsaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // Verilen günün hafta sonu (Cumartesi veya Pazar) olup olmadığını kontrol eden fonksiyon
        bool IsWeekend(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Dolar")]

        public async Task<IActionResult> GetExchangeRates([FromQuery] int daysAgo)
        {
            // Geçerli tarihi alın
            var endDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            // Girilen değere göre başlangıç tarihini hesaplayın
            var startDate = DateTime.Now.Date.AddDays(-daysAgo).ToString("dd-MM-yyyy");


            // TCMB API'sine yapılacak isteğin URL'sini oluşturun
            string url = $"https://evds2.tcmb.gov.tr/service/evds/series=TP.DK.USD.S.YTL-TP.DK.EUR.S.YTL-TP.DK.CHF.S.YTL-TP.DK.GBP.S.YTL-TP.DK.JPY.S.YTL&startDate={startDate}&endDate={endDate}&type=xml&key=Mgp1hyATqu";

            // API'den yanıt almak için HTTP GET isteği gönderin
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // API yanıtını okuyun
                string xmlString = await response.Content.ReadAsStringAsync();

                // XML yanıtını işlemek için XDocument kullanın
                XDocument doc = XDocument.Parse(xmlString);

                // "items" etiketleri altındaki döviz kurlarını seçin ve işleyin
                var exchangeRates = doc.Descendants("items")
                    // Boş olmayan ve "0" olmayan değerlere sahip olanları filtreleyin
                    .Where(item => !string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) && item.Element("TP_DK_CHF_S_YTL").Value != "0")
                    .Select(item => new
                    {
                        // Tarih ve İsviçre Frangı (CHF) kuru bilgilerini alın
                        date = item.Element("Tarih").Value,
                        usd = string.IsNullOrEmpty(item.Element("TP_DK_USD_S_YTL").Value) ? "0" : item.Element("TP_DK_USD_S_YTL").Value,

                    })
                    // Hafta sonu günlerini filtreleyin
                    .Where(rate => !IsWeekend(DateTime.ParseExact(rate.date, "dd-MM-yyyy", CultureInfo.InvariantCulture).DayOfWeek))
                    .ToList();

                // Yeni bir XML belgesi oluşturun
                XElement root = new XElement("ExchangeRates",
                    from ex in exchangeRates
                    select new XElement("items",
                        new XElement("date", ex.date),
                         new XElement("usd", ex.usd)
                    )
                );

                // XML belgesini içerik olarak dönün
                return Content(root.ToString(), "application/xml");
            }
            else
            {
                // API isteği başarısız olursa uygun bir hata kodu ve mesaj dönün
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Euro")]

        public async Task<IActionResult> GetExchangeRates1([FromQuery] int daysAgo)
        {
            // Geçerli tarihi alın
            var endDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            // Girilen değere göre başlangıç tarihini hesaplayın
            var startDate = DateTime.Now.Date.AddDays(-daysAgo).ToString("dd-MM-yyyy");

            // TCMB API'sine yapılacak isteğin URL'sini oluşturun
            string url = $"https://evds2.tcmb.gov.tr/service/evds/series=TP.DK.USD.S.YTL-TP.DK.EUR.S.YTL-TP.DK.CHF.S.YTL-TP.DK.GBP.S.YTL-TP.DK.JPY.S.YTL&startDate={startDate}&endDate={endDate}&type=xml&key=Mgp1hyATqu";

            // API'den yanıt almak için HTTP GET isteği gönderin
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // API yanıtını okuyun
                string xmlString = await response.Content.ReadAsStringAsync();

                // XML yanıtını işlemek için XDocument kullanın
                XDocument doc = XDocument.Parse(xmlString);

                // "items" etiketleri altındaki döviz kurlarını seçin ve işleyin
                var exchangeRates = doc.Descendants("items")
                    // Boş olmayan ve "0" olmayan değerlere sahip olanları filtreleyin
                    .Where(item => !string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) && item.Element("TP_DK_CHF_S_YTL").Value != "0")
                    .Select(item => new
                    {
                        // Tarih ve İsviçre Frangı (CHF) kuru bilgilerini alın
                        date = item.Element("Tarih").Value,
                        eur = string.IsNullOrEmpty(item.Element("TP_DK_EUR_S_YTL").Value) ? "0" : item.Element("TP_DK_EUR_S_YTL").Value,


                    })
                    // Hafta sonu günlerini filtreleyin
                    .Where(rate => !IsWeekend(DateTime.ParseExact(rate.date, "dd-MM-yyyy", CultureInfo.InvariantCulture).DayOfWeek))
                    .ToList();

                // Yeni bir XML belgesi oluşturun
                XElement root = new XElement("ExchangeRates",
                    from ex in exchangeRates
                    select new XElement("items",
                        new XElement("date", ex.date),
                           new XElement("eur", ex.eur)
                    )
                );

                // XML belgesini içerik olarak dönün
                return Content(root.ToString(), "application/xml");
            }
            else
            {
                // API isteği başarısız olursa uygun bir hata kodu ve mesaj dönün
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [HttpGet("IsvicreFrangi")]
        public async Task<IActionResult> GetExchangeRates2([FromQuery] int daysAgo)
        {
            // Geçerli tarihi alın
            var endDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            // Girilen değere göre başlangıç tarihini hesaplayın
            var startDate = DateTime.Now.Date.AddDays(-daysAgo).ToString("dd-MM-yyyy");

            // TCMB API'sine yapılacak isteğin URL'sini oluşturun
            string url = $"https://evds2.tcmb.gov.tr/service/evds/series=TP.DK.USD.S.YTL-TP.DK.EUR.S.YTL-TP.DK.CHF.S.YTL-TP.DK.GBP.S.YTL-TP.DK.JPY.S.YTL&startDate={startDate}&endDate={endDate}&type=xml&key=Mgp1hyATqu";

            // API'den yanıt almak için HTTP GET isteği gönderin
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // API yanıtını okuyun
                string xmlString = await response.Content.ReadAsStringAsync();

                // XML yanıtını işlemek için XDocument kullanın
                XDocument doc = XDocument.Parse(xmlString);

                // "items" etiketleri altındaki döviz kurlarını seçin ve işleyin
                var exchangeRates = doc.Descendants("items")
                    // Boş olmayan ve "0" olmayan değerlere sahip olanları filtreleyin
                    .Where(item => !string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) && item.Element("TP_DK_CHF_S_YTL").Value != "0")
                    .Select(item => new
                    {
                        // Tarih ve İsviçre Frangı (CHF) kuru bilgilerini alın
                        date = item.Element("Tarih").Value,
                        chf = item.Element("TP_DK_CHF_S_YTL").Value,
                    })
                    // Hafta sonu günlerini filtreleyin
                    .Where(rate => !IsWeekend(DateTime.ParseExact(rate.date, "dd-MM-yyyy", CultureInfo.InvariantCulture).DayOfWeek))
                    .ToList();

                // Yeni bir XML belgesi oluşturun
                XElement root = new XElement("ExchangeRates",
                    from ex in exchangeRates
                    select new XElement("items",
                        new XElement("date", ex.date),
                        new XElement("che", ex.chf)
                    )
                );

                // XML belgesini içerik olarak dönün
                return Content(root.ToString(), "application/xml");
            }
            else
            {
                // API isteği başarısız olursa uygun bir hata kodu ve mesaj dönün
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [HttpGet("IngilizSterlini")]

        public async Task<IActionResult> GetExchangeRates3([FromQuery] int daysAgo)
        {
            // Geçerli tarihi alın
            var endDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            // Girilen değere göre başlangıç tarihini hesaplayın
            var startDate = DateTime.Now.Date.AddDays(-daysAgo).ToString("dd-MM-yyyy");

            // TCMB API'sine yapılacak isteğin URL'sini oluşturun
            string url = $"https://evds2.tcmb.gov.tr/service/evds/series=TP.DK.USD.S.YTL-TP.DK.EUR.S.YTL-TP.DK.CHF.S.YTL-TP.DK.GBP.S.YTL-TP.DK.JPY.S.YTL&startDate={startDate}&endDate={endDate}&type=xml&key=Mgp1hyATqu";

            // API'den yanıt almak için HTTP GET isteği gönderin
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // API yanıtını okuyun
                string xmlString = await response.Content.ReadAsStringAsync();

                // XML yanıtını işlemek için XDocument kullanın
                XDocument doc = XDocument.Parse(xmlString);

                // "items" etiketleri altındaki döviz kurlarını seçin ve işleyin
                var exchangeRates = doc.Descendants("items")
                    // Boş olmayan ve "0" olmayan değerlere sahip olanları filtreleyin
                    .Where(item => !string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) && item.Element("TP_DK_CHF_S_YTL").Value != "0")
                    .Select(item => new
                    {
                        // Tarih ve İsviçre Frangı (CHF) kuru bilgilerini alın
                        date = item.Element("Tarih").Value,
                        gbp = string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) ? "0" : item.Element("TP_DK_GBP_S_YTL").Value,
                    })
                    // Hafta sonu günlerini filtreleyin
                    .Where(rate => !IsWeekend(DateTime.ParseExact(rate.date, "dd-MM-yyyy", CultureInfo.InvariantCulture).DayOfWeek))
                    .ToList();

                // Yeni bir XML belgesi oluşturun
                XElement root = new XElement("ExchangeRates",
                    from ex in exchangeRates
                    select new XElement("items",
                        new XElement("date", ex.date),
                        new XElement("gbp", ex.gbp)
                    )
                );

                // XML belgesini içerik olarak dönün
                return Content(root.ToString(), "application/xml");
            }
            else
            {
                // API isteği başarısız olursa uygun bir hata kodu ve mesaj dönün
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        [HttpGet("JaponYeni")]

        public async Task<IActionResult> GetExchangeRates4([FromQuery] int daysAgo)
        {
            // Geçerli tarihi alın
            var endDate = DateTime.Now.Date.ToString("dd-MM-yyyy");
            // Girilen değere göre başlangıç tarihini hesaplayın
            var startDate = DateTime.Now.Date.AddDays(-daysAgo).ToString("dd-MM-yyyy");

            // TCMB API'sine yapılacak isteğin URL'sini oluşturun
            string url = $"https://evds2.tcmb.gov.tr/service/evds/series=TP.DK.USD.S.YTL-TP.DK.EUR.S.YTL-TP.DK.CHF.S.YTL-TP.DK.GBP.S.YTL-TP.DK.JPY.S.YTL&startDate={startDate}&endDate={endDate}&type=xml&key=Mgp1hyATqu";

            // API'den yanıt almak için HTTP GET isteği gönderin
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // API yanıtını okuyun
                string xmlString = await response.Content.ReadAsStringAsync();

                // XML yanıtını işlemek için XDocument kullanın
                XDocument doc = XDocument.Parse(xmlString);

                // "items" etiketleri altındaki döviz kurlarını seçin ve işleyin
                var exchangeRates = doc.Descendants("items")
                    // Boş olmayan ve "0" olmayan değerlere sahip olanları filtreleyin
                    .Where(item => !string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) && item.Element("TP_DK_CHF_S_YTL").Value != "0")
                    .Select(item => new
                    {
                        // Tarih ve İsviçre Frangı (CHF) kuru bilgilerini alın
                        date = item.Element("Tarih").Value,
                        jpy = string.IsNullOrEmpty(item.Element("TP_DK_CHF_S_YTL").Value) ? "0" : item.Element("TP_DK_JPY_S_YTL").Value,
                    })
                    // Hafta sonu günlerini filtreleyin
                    .Where(rate => !IsWeekend(DateTime.ParseExact(rate.date, "dd-MM-yyyy", CultureInfo.InvariantCulture).DayOfWeek))
                    .ToList();

                // Yeni bir XML belgesi oluşturun
                XElement root = new XElement("ExchangeRates",
                    from ex in exchangeRates
                    select new XElement("items",
                        new XElement("date", ex.date),
                     new XElement("jpy", ex.jpy)
                    )
                );

                // XML belgesini içerik olarak dönün
                return Content(root.ToString(), "application/xml");
            }
            else
            {
                // API isteği başarısız olursa uygun bir hata kodu ve mesaj dönün
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}