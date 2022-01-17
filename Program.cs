using System;
using System.Net.Http;
using System.Net.Http.Headers;
using NW = Newtonsoft.Json;
using MS = System.Text.Json;
using System.Collections.Generic;
using var client = new HttpClient();
var key = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJtZXQwMS5hcGlrZXkiLCJpc3MiOiJJRVMgUExBSUFVTkRJIEJISSBJUlVOIiwiZXhwIjoyMjM4MTMxMDAyLCJ2ZXJzaW9uIjoiMS4wLjAiLCJpYXQiOjE2NDE5NzM4MDcsImVtYWlsIjoiaWtiZHZAcGxhaWF1bmRpLm5ldCJ9.IofLYTTBr0PZoiLxmVzrqBU6vYWnoQX8Bi2SorSrvnzinBIG28AutQL3M6CEvLWstteyX74gQzCltKxZYrWUYkrsi9GXWsMzz20TiiSkz1D2KarxLiV5a4yFN71NybjYG_XHEWmnkoMIZmlFQ6O3f4ixyFdSFmLEVjI1-2Ud4XD8LNm035o_8_kkFxKYLYhElnn8wwC44tt5CeT9efMOxQLKa9JrsHUMapypWOybXIeSyScRAgjN8dMySX6IZx7YX6Wt3-buzFxXmBQAlmjvNULWQ0r2VPHnthETr72RWLT1hYhXxOaLdBEnGe6F7hiwTHonU9fy_wBkr2i697qGTA";
client.DefaultRequestHeaders.Add("User-Agent", "mi consola");
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.DefaultRequestHeaders.Add("Authorization", "Bearer " + key);
var urlRegiones = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones";
HttpResponseMessage response4 = await client.GetAsync(urlRegiones);
var resp4 = await response4.Content.ReadAsStringAsync();
dynamic jsonObject4 = NW.JsonConvert.DeserializeObject(resp4);
dynamic pp5 = jsonObject4;
while (true)
{
    foreach (var item in pp5)
        Console.WriteLine($"{item.regionZoneId}");
    Console.WriteLine("**********************************");
    Console.WriteLine("Introduce la zona");
    var zona = Console.ReadLine();
    Console.WriteLine("**********************************");
    Console.WriteLine("Localidades");
    var urlLocalidades = $"https://api.euskadi.eus/euskalmet/geo/regions/basque_country/zones/{zona}/locations";
    HttpResponseMessage response5 = await client.GetAsync(urlLocalidades);
    var resp5 = await response5.Content.ReadAsStringAsync();
    dynamic jsonObject5 = NW.JsonConvert.DeserializeObject(resp5);
    dynamic pp6 = jsonObject5;
    foreach (var item in pp6)
        Console.WriteLine($"{item.regionZoneLocationId}");
    Console.WriteLine("**********************************");
    Console.WriteLine("Introduce la localidad");
    var municipio = Console.ReadLine();
    var Dia = DateTime.Today.Day;
    var diaHoy = "";
    if (Convert.ToInt32(Dia) / 10 == 0)
    {
        diaHoy = "0" + Convert.ToInt32(Dia);
    }
    else
    {
        diaHoy = "" + Convert.ToInt32(Dia);
    }
    var AñoHoy = DateTime.Today.Year;
    var mes = DateTime.Today.Month;
    var mesHoy = "";
    if (Convert.ToInt32(mes) / 10 == 0)
    {
        mesHoy = "0" + Convert.ToInt32(mes);
    }
    else
    {
        mesHoy = "" + Convert.ToInt32(mes);
    }
    var urlLocalizacionForecast = $"https://api.euskadi.eus/euskalmet/weather/regions/basque_country/zones/{zona}/locations/{municipio}/forecast/trends/measures/at/{AñoHoy}/{mesHoy}/{diaHoy}/for/{AñoHoy}{mesHoy}{diaHoy}";
    Console.WriteLine(urlLocalizacionForecast);
    var urlEstaciones = "https://api.euskadi.eus/euskalmet/stations";
    var url = "https://api.euskadi.eus/euskalmet/readings/forStation/C071/R0BU/measures/measuresForAir/temperature/at/2022/01/14/08";
    HttpResponseMessage response = await client.GetAsync(url);
    HttpResponseMessage response2 = await client.GetAsync(urlEstaciones);
    HttpResponseMessage response3 = await client.GetAsync(urlLocalizacionForecast);
    response.EnsureSuccessStatusCode();
    var resp = await response.Content.ReadAsStringAsync();
    var resp2 = await response2.Content.ReadAsStringAsync();
    var resp3 = await response3.Content.ReadAsStringAsync();
    dynamic jsonObject3 = NW.JsonConvert.DeserializeObject(resp3);
    var tiempoPretry=jsonObject3;
    try
    {
        // var marcatiempo=tiempoPretry.trends.set[0].range+"";
        // Console.WriteLine(marcatiempo.substring(0,10));
        dynamic pp = jsonObject3.trends.set[0].temperature;
        dynamic pp1 = jsonObject3.trends.set[0].precipitation;
        dynamic pp2 = jsonObject3.trends.set[0].windspeed;
        dynamic pp3 = jsonObject3.trends.set[0].symbolSet.weather.nameByLang.SPANISH;
        Console.WriteLine($" ");
        Console.WriteLine($"****************************************************************");
        Console.WriteLine($"Temperatura de {municipio} = {pp.value} Cº ");
        Console.WriteLine($"Precitipacion acumulada en {municipio} = {pp1.value} ml ");
        Console.WriteLine($"Velocidad del Viento en {municipio}= {pp2.value} Km/h");
        Console.WriteLine($"Descripcion del tiempo en  {municipio} = {pp3} ");
        Console.WriteLine($"****************************************************************");
        Console.WriteLine("====");
        //Aqui El Insert
    }
    catch (Exception e)
    {
        Console.WriteLine("Lo siento no hay datos en Euskalmet");
    }
}
//dynamic jsonObject = NW.JsonConvert.DeserializeObject(resp);
//Console.WriteLine(jsonObject.values);
//**************************************************************//
public class Temperature
{
    public double value { get; set; }
    public string unit { get; set; }
}

public class Precipitation
{
    public int value { get; set; }
    public string unit { get; set; }
}

public class Winddirection
{
    public int value { get; set; }
    public string unit { get; set; }
    public string cardinalpoint { get; set; }
}

public class Windspeed
{
    public double value { get; set; }
    public string unit { get; set; }
}

public class NameByLang
{
    public string SPANISH { get; set; }
    public string BASQUE { get; set; }
}

public class DescriptionByLang
{
    public string SPANISH { get; set; }
    public string BASQUE { get; set; }
}

public class Weather
{
    public string id { get; set; }
    public string path { get; set; }
    public NameByLang nameByLang { get; set; }
    public DescriptionByLang descriptionByLang { get; set; }
}

public class SymbolSet
{
    public Weather weather { get; set; }
}

public class ShortDescription
{
    public string SPANISH { get; set; }
    public string BASQUE { get; set; }
}

public class Set
{
    public string range { get; set; }
    public Temperature temperature { get; set; }
    public Precipitation precipitation { get; set; }
    public Winddirection winddirection { get; set; }
    public Windspeed windspeed { get; set; }
    public SymbolSet symbolSet { get; set; }
    public ShortDescription shortDescription { get; set; }
}

public class Trends
{
    public List<Set> set { get; set; }
}

public class Root4
{
    public string oid { get; set; }
    public int numericId { get; set; }
    public int entityVersion { get; set; }
    public DateTime at { get; set; }
    public DateTime @for { get; set; }
    public Trends trends { get; set; }
}





//***********************************************************//
public class Slot
{
    public string range { get; set; }
    public string rangeDesc { get; set; }
    public string lowerEndPointDesc { get; set; }
    public string upperEndPointDesc { get; set; }
}

public class Root3
{
    public string typeId { get; set; }
    public string oid { get; set; }
    public int numericId { get; set; }
    public int entityVersion { get; set; }
    public string station { get; set; }
    public string sensor { get; set; }
    public string measureType { get; set; }
    public string measure { get; set; }
    public string dateRange { get; set; }
    public List<Slot> slots { get; set; }
    public List<double> values { get; set; }
}


public class Root2
{
    public string key { get; set; }
    public string stationId { get; set; }
    public DateTime snapshotDate { get; set; }
}






//Root myDeserializedClass = NW.JsonConvert.DeserializeObject<Root>(resp);
//Root2 myDeserializedClass2 = NW.JsonConvert.DeserializeObject<Root2>(resp2);

//foreach (var item in myDeserializedClass.values)
//    Console.WriteLine($" {item} ");

//Console.WriteLine("====");
//dynamic jsonObject2 = NW.JsonConvert.DeserializeObject(resp2);


//foreach (var item in jsonObject2)
//    Console.WriteLine($" {item.stationId} ");

//Root4 myDeserializedClass3 = NW.JsonConvert.DeserializeObject<Root4>(resp3);