using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using NW = Newtonsoft.Json;
using MS = System.Text.Json;
using System.Collections.Generic;
using Tiempo.Models;
using var client = new HttpClient();


//  async static void Main(string[] args)
// {
//     int intervalo = 5 * 1000;
//     try
//     {
//         //intervalo = 1000 * Int16.Parse(args[0]);
//         intervalo = 1000 * Int16.Parse(
//             Environment.GetEnvironmentVariable("SEGUNDOS")
//             );
//     }
//     catch { }
//     while (true)
//     {
//         var t = await Task.Run(() =>  recogerDatos());
//         // Esperamos que acabe la tarea
//         t.Wait();
//         // Esperamos el intervalo
//         System.Threading.Thread.Sleep(intervalo);
//     }


// }

//await recogerDatos();
async void recogerDatos()
{
    //********JWT********
    var key = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJtZXQwMS5hcGlrZXkiLCJpc3MiOiJJRVMgUExBSUFVTkRJIEJISSBJUlVOIiwiZXhwIjoyMjM4MTMxMDAyLCJ2ZXJzaW9uIjoiMS4wLjAiLCJpYXQiOjE2NDE5NzM3OTksImVtYWlsIjoiaWtiZWNAcGxhaWF1bmRpLm5ldCJ9.YPuReD7iqyLlg0zu7DimIWYHlENKM1JwjfF8wYRygLyNnEvnGrUOD-65607nHjlpCYH1zMc-xjLqcTn4oaOXaZy6cIP005CMLxDLXLZNS1brmHihJdAQ2-fMSjBSb1dD0zcb8arYn-lLzVb1GkRWaa6LT4t4GeaTokwTwel_8BtBPOVLOJx8WqILXSJk-Fy53H-BES_ygoqzWu4hOiasRgDKN-IiZvyBReixwXSq92fljibuXYqRFyrXX23yQxBqwiqbzI0DuQvmMKJUKIwYAwu_vrpWkRRNWHLCVlZ-P4qYScEwO0wVYCHXBGPbcG23GQyo2oD9_5kdP3LwEu9pTg";
    client.DefaultRequestHeaders.Add("User-Agent", "mi consola");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + key);
    //*******************
    var urlRegiones = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones";
    HttpResponseMessage response4 = await client.GetAsync(urlRegiones);
    var resp4 = await response4.Content.ReadAsStringAsync();
    dynamic jsonObject4 = NW.JsonConvert.DeserializeObject(resp4);
    dynamic pp5 = jsonObject4;
    foreach (var zonas in pp5)
    {
        Console.WriteLine($"{zonas.regionZoneId}");
        // Console.WriteLine("**********************************");
        // Console.WriteLine("Introduce la zona");
        // var zona = Console.ReadLine();
        // Console.WriteLine("**********************************");
        // Console.WriteLine("Localidades");
        var urlLocalidades = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones/{zonas.regionZoneId}/locations";
        HttpResponseMessage response5 = await client.GetAsync(urlLocalidades);
        var resp5 = await response5.Content.ReadAsStringAsync();
        dynamic jsonObject5 = NW.JsonConvert.DeserializeObject(resp5);
        dynamic pp6 = jsonObject5;
        foreach (var item in pp6)
        {
            Console.WriteLine($"{item.regionZoneLocationId}");
            // Console.WriteLine("**********************************");
            // Console.WriteLine("Introduce la localidad");
            // var municipio = Console.ReadLine();
            var Dia = DateTime.Today.Day;
            var diaHoy = "";
            var AñoHoy = DateTime.Today.Year;
            var mes = DateTime.Today.Month;
            var mesHoy = "";
            if (Convert.ToInt32(Dia) / 10 == 0)
            {
                diaHoy = "0" + Convert.ToInt32(Dia);
            }
            else
            {
                diaHoy = "" + Convert.ToInt32(Dia);
            }
            if (Convert.ToInt32(mes) / 10 == 0)
            {
                mesHoy = "0" + Convert.ToInt32(mes);
            }
            else
            {
                mesHoy = "" + Convert.ToInt32(mes);
            }
            var urlLocalizacionForecast = $"https://api.euskadi.eus/euskalmet/weather/regions/basque_country/zones/{zonas.regionZoneId}/locations/{item.regionZoneLocationId}/forecast/trends/measures/at/{AñoHoy}/{mesHoy}/{diaHoy}/for/{AñoHoy}{mesHoy}{diaHoy}";
            Console.WriteLine(urlLocalizacionForecast);
            try
            {
                HttpResponseMessage response3 = await client.GetAsync(urlLocalizacionForecast);
                var resp3 = await response3.Content.ReadAsStringAsync();
                dynamic jsonObject3 = NW.JsonConvert.DeserializeObject(resp3);
                var tiempoPretry = jsonObject3.trends.set;

                var hora = Convert.ToInt32(DateTime.Now.Hour) - 1;
                var horaAhora = "";
                if (hora / 10 == 0)
                {
                    horaAhora = "0" + hora;
                }
                else
                {
                    horaAhora = "" + hora;
                }
                var valor = 0;
                var i = 0;
                var stringComp = $"LocalTime:[{horaAhora}:00:00:000..{horaAhora}:59:59:999]";
                Console.WriteLine(stringComp);
                foreach (var x in tiempoPretry)
                {
                    if (Convert.ToString(x.range) == stringComp) valor = i;
                    i++;
                }
                dynamic pp = jsonObject3.trends.set[valor].temperature;
                dynamic pp1 = jsonObject3.trends.set[valor].precipitation;
                dynamic pp2 = jsonObject3.trends.set[valor].windspeed;
                dynamic pp3 = jsonObject3.trends.set[valor].symbolSet.weather.nameByLang.SPANISH;
                Console.WriteLine($" ");
                Console.WriteLine($"****************************************************************");
                Console.WriteLine($"{jsonObject3.trends.set[valor].range}");
                Console.WriteLine($"Temperatura de {item.regionZoneLocationId} = {pp.value} ºC ");
                Console.WriteLine($"Precitipacion acumulada en {item.regionZoneLocationId} = {pp1.value} ml ");
                Console.WriteLine($"Velocidad del Viento en {item.regionZoneLocationId}= {pp2.value} Km/h");
                Console.WriteLine($"Descripcion del tiempo en  {item.regionZoneLocationId} = {pp3} ");
                Console.WriteLine($"****************************************************************");
                Console.WriteLine("====");
                //Aqui El Insert
                using (var db = new TiempoContext())
                {
                    try
                    {
                        string localizacion = item.regionZoneLocationId + "";
                        //var a1 = new Tiempo2 { Municipio = item.regionZoneLocationId, Region = zonas.regionZoneId, Temperatura = pp.value, VelocidadViento = pp2.value, DescripcionTiempo = pp3, Precipitaciones = pp1.value, ultimaHora=jsonObject3.trends.set[valor].range, PathImg=jsonObject3.trends.set[valor].symbolset.weather.path}; db.Tiempo.Update(a1);

                        var row = db.Tiempo2.Where(a => a.Municipio == localizacion).Single();
                        row.ultimaHora = jsonObject3.trends.set[valor].range;
                        row.VelocidadViento = pp2.value;
                        row.Temperatura = pp.value;
                        row.DescripcionTiempo = pp3;
                        row.PathImg = jsonObject3.trends.set[valor].symbolSet.weather.path;
                        db.SaveChanges();
                    }
                    catch (Exception p)
                    {
                        Console.WriteLine("No se ha podido guardar por " + p);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Lo siento no hay datos en Euskalmet");
            }
        }
    }
}


