using Newtonsoft.Json;
using TikTokCategoryExtractor;
using TikTokCategoryExtractor.Helpers;
using TikTokCategoryExtractor.Requests;
using TikTokCategoryExtractor.Responses;

internal enum Command
{
    GenerateReport = 1,
    GenerateEnum = 2,
    GenerateUnorderedList = 3,
    FindMatchingKeys = 4,
    BuildCategoryBreadCrumbs = 5,
    GenerateFieldDescriptions = 6,
    DeleteDuplicateProducts = 7,
}

internal class Program
{
    private static string _apiVersion;
    private static Uri _baseURI;
    private static string _accessToken;
    private static string _appKey;
    private static string _appSecret;
    private static TikTokAPIClient _client;
    private static List<ProductAttribute> _attributes;
    private static Command _command;
    private static string _fileName;
    private static string _filePath;

    private static void InitializeProperties()
    {
        _apiVersion = "";
        _baseURI = new Uri("https://open-api.tiktokglobalshop.com");
        _accessToken = "";
        _appKey = "";
        _appSecret = "";
        _client = new TikTokAPIClient(_baseURI, _accessToken, _appKey, _appSecret, _apiVersion);
        _attributes = new List<ProductAttribute>();
        _command = Command.DeleteDuplicateProducts;
        _fileName = "OutputFile.csv";
        _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", _fileName);
    }

    private static void Main(string[] args)
    {
        Console.Clear();
        InitializeProperties();

        if (_command == Command.GenerateReport)
        {
            var tikTokCategories = _client.SendRequest<TikTokCategories>(HttpMethod.Get,
                        "/api/products/categories", null, "Failed to get categories");

            if (tikTokCategories.IsSuccess && tikTokCategories?.Data?.CategoryList != null)
            {
                GenerateCategoryReport(tikTokCategories);
            }
        }
        else if (_command == Command.GenerateEnum)
        {
            string enumNamesInput = "China:Malaysia:Philippines:Australia:Singapore";
            string enumValuesInput = "1000850:1000852:1000855:1000861:1000862";
            string enumName = "CountryOfOriginChinaAndMalaysiaToSingapore";

            ProductAttributesHelper.GenerateEnum(enumNamesInput, enumValuesInput, _filePath, enumName);
        }
        else if (_command == Command.GenerateUnorderedList)
        {
            string listInput = "Apple iPad 10.2 (2019):Apple iPad 10.2 (2020):Apple iPad 10.2 (2021):Apple iPad 2 CDMA:Apple iPad 2 Wi-Fi:Apple iPad 2 Wi-Fi + 3G:Apple iPad 3 Wi-Fi:Apple iPad 3 Wi-Fi + Cellular:Apple iPad 4 Wi-Fi:Apple iPad 4 Wi-Fi + Cellular:Apple iPad 9.7 (2017):Apple iPad 9.7 (2018):Apple iPad Air:Apple iPad Air (2019):Apple iPad Air (2020):Apple iPad Air (2022):Apple iPad Air 2:Apple iPad mini (2019):Apple iPad mini (2021):Apple iPad mini 2:Apple iPad mini 3:Apple iPad mini 4 (2015):Apple iPad mini Wi-Fi:Apple iPad mini Wi-Fi + Cellular:Apple iPad Pro 10.5 (2017):Apple iPad Pro 11 (2018):Apple iPad Pro 11 (2020):Apple iPad Pro 11 (2021):Apple iPad Pro 12.9 (2015):Apple iPad Pro 12.9 (2017):Apple iPad Pro 12.9 (2018):Apple iPad Pro 12.9 (2020):Apple iPad Pro 12.9 (2021):Apple iPad Pro 9.7 (2016):Apple iPad Wi-Fi:Apple iPad Wi-Fi + 3G:vivo Pad:Realme Pad:Realme Pad Mini:Realme Pad X:Samsung Ativ Tab P8510:Samsung Galaxy Note 10.1 (2014):Samsung Galaxy Note 10.1 N8000:Samsung Galaxy Note 10.1 N8010:Samsung Galaxy Note 8.0:Samsung Galaxy Note 8.0 Wi-Fi:Samsung Galaxy Note LTE 10.1 N8020:Samsung Galaxy Note Pro 12.2:Samsung Galaxy Note Pro 12.2 3G:Samsung Galaxy Note Pro 12.2 LTE:Samsung Galaxy Tab 10.1 LTE I905:Samsung Galaxy Tab 10.1 P7510:Samsung Galaxy Tab 2 10.1 CDMA:Samsung Galaxy Tab 2 10.1 P5100:Samsung Galaxy Tab 2 10.1 P5110:Samsung Galaxy Tab 2 7.0 I705:Samsung Galaxy Tab 2 7.0 P3100:Samsung Galaxy Tab 2 7.0 P3110:Samsung Galaxy Tab 3 10.1 P5200:Samsung Galaxy Tab 3 10.1 P5210:Samsung Galaxy Tab 3 10.1 P5220:Samsung Galaxy Tab 3 7.0:Samsung Galaxy Tab 3 7.0 WiFi:Samsung Galaxy Tab 3 8.0:Samsung Galaxy Tab 3 Lite 7.0:Samsung Galaxy Tab 3 Lite 7.0 3G:Samsung Galaxy Tab 3 Lite 7.0 VE:Samsung Galaxy Tab 3 Plus 10.1 P8220:Samsung Galaxy Tab 3 V:Samsung Galaxy Tab 4 10.1:Samsung Galaxy Tab 4 10.1 (2015):Samsung Galaxy Tab 4 10.1 3G:Samsung Galaxy Tab 4 10.1 LTE:Samsung Galaxy Tab 4 7.0:Samsung Galaxy Tab 4 7.0 3G:Samsung Galaxy Tab 4 7.0 LTE:Samsung Galaxy Tab 4 8.0:Samsung Galaxy Tab 4 8.0 (2015):Samsung Galaxy Tab 4 8.0 3G:Samsung Galaxy Tab 4 8.0 LTE:Samsung Galaxy Tab 4G LTE:Samsung Galaxy Tab 7.7 LTE I815:Samsung Galaxy Tab 8.9 4G P7320T:Samsung Galaxy Tab 8.9 LTE I957:Samsung Galaxy Tab 8.9 P7300:Samsung Galaxy Tab 8.9 P7310:Samsung Galaxy Tab A 10.1 (2016):Samsung Galaxy Tab A 10.1 (2019):Samsung Galaxy Tab A 10.5:Samsung Galaxy Tab A 7.0 (2016):Samsung Galaxy Tab A 8.0 & S Pen (2015):Samsung Galaxy Tab A 8.0 & S Pen (2019):Samsung Galaxy Tab A 8.0 (2015):Samsung Galaxy Tab A 8.0 (2017):Samsung Galaxy Tab A 8.0 (2018):Samsung Galaxy Tab A 8.0 (2019):Samsung Galaxy Tab A 8.4 (2020):Samsung Galaxy Tab A 9.7:Samsung Galaxy Tab A 9.7 & S Pen:Samsung Galaxy Tab A7 10.4 (2020):Samsung Galaxy Tab A7 Lite:Samsung Galaxy Tab A8 10.5 (2021):Samsung Galaxy Tab Active:Samsung Galaxy Tab Active 2:Samsung Galaxy Tab Active LTE:Samsung Galaxy Tab Active Pro:Samsung Galaxy Tab Active3:Samsung Galaxy Tab Active4 Pro:Samsung Galaxy Tab Advanced2:Samsung Galaxy Tab CDMA P100:Samsung Galaxy Tab E 8.0:Samsung Galaxy Tab E 9.6:Samsung Galaxy Tab J:Samsung Galaxy Tab Pro 10.1:Samsung Galaxy Tab Pro 10.1 LTE:Samsung Galaxy Tab Pro 12.2:Samsung Galaxy Tab Pro 12.2 3G:Samsung Galaxy Tab Pro 12.2 LTE:Samsung Galaxy Tab Pro 8.4:Samsung Galaxy Tab Pro 8.4 3G/LTE:Samsung Galaxy Tab S 10.5:Samsung Galaxy Tab S 10.5 LTE:Samsung Galaxy Tab S 8.4:Samsung Galaxy Tab S 8.4 LTE:Samsung Galaxy Tab S2 8.0:Samsung Galaxy Tab S2 9.7:Samsung Galaxy Tab S3 9.7:Samsung Galaxy Tab S4 10.5:Samsung Galaxy Tab S5e:Samsung Galaxy Tab S6:Samsung Galaxy Tab S6 5G:Samsung Galaxy Tab S6 Lite:Samsung Galaxy Tab S6 Lite (2022):Samsung Galaxy Tab S7:Samsung Galaxy Tab S7 FE:Samsung Galaxy Tab S7+:Samsung Galaxy Tab S8:Samsung Galaxy Tab S8 Ultra:Samsung Galaxy Tab S8+:Samsung Galaxy Tab T-Mobile T849:Samsung Galaxy View:Samsung Galaxy View2:Samsung Google Nexus 10 P8110:Samsung P1000 Galaxy Tab:Samsung P1010 Galaxy Tab Wi-Fi:Samsung P6200 Galaxy Tab 7.0 Plus:Samsung P6210 Galaxy Tab 7.0 Plus:Samsung P6800 Galaxy Tab 7.7:Samsung P6810 Galaxy Tab 7.7:Samsung P7100 Galaxy Tab 10.1v:Samsung P7500 Galaxy Tab 10.1 3G:Oppo Pad:Oppo Pad Air:Xiaomi Mi Pad 2:Xiaomi Mi Pad 3:Xiaomi Mi Pad 4:Xiaomi Mi Pad 4 Plus:Xiaomi Mi Pad 7.9:Xiaomi Pad 5:Xiaomi Pad 5 Pro:Xiaomi Pad 5 Pro 12.4:Xiaomi Redmi Pad:Nokia N1:Nokia N800:Nokia N810:Nokia T10:Nokia T20:Nokia T21:Asus Fonepad:Asus Fonepad 7:Asus Fonepad 7 (2014):Asus Fonepad 7 FE171CG:Asus Fonepad 7 FE375CG:Asus Fonepad 7 FE375CL:Asus Fonepad 7 FE375CXG:Asus Fonepad 8 FE380CG:Asus Fonepad Note FHD6:Asus Google Nexus 7:Asus Google Nexus 7 (2013):Asus Google Nexus 7 Cellular:Asus Memo Pad 10:Asus Memo Pad 10 ME103K:Asus Memo Pad 7 ME176C:Asus Memo Pad 7 ME572C:Asus Memo Pad 7 ME572CL:Asus Memo Pad 8 ME180A:Asus Memo Pad 8 ME181C:Asus Memo Pad 8 ME581CL:Asus Memo Pad FHD10:Asus Memo Pad HD7 16 GB:Asus Memo Pad HD7 8 GB:Asus Memo Pad ME172V:Asus Memo Pad Smart 10:Asus Transformer Pad Infinity 700:Asus Transformer Pad Infinity 700 3G:Asus Transformer Pad Infinity 700 LTE:Asus Transformer Pad TF103C:Asus Transformer Pad TF300T:Asus Transformer Pad TF300TG:Asus Transformer Pad TF303CL:Asus Transformer Pad TF701T:Asus Transformer Prime TF700T:Asus Zenpad 10 Z300C:Asus Zenpad 10 Z300M:Asus Zenpad 3 8.0 Z581KL:Asus Zenpad 3S 10 Z500KL:Asus Zenpad 3S 10 Z500M:Asus Zenpad 3s 8.0 Z582KL:Asus Zenpad 7.0 Z370CG:Asus Zenpad 8.0 Z380C:Asus Zenpad 8.0 Z380KL:Asus Zenpad 8.0 Z380M:Asus Zenpad C 7.0:Asus Zenpad C 7.0 Z170MG:Asus Zenpad S 8.0 Z580C:Asus Zenpad S 8.0 Z580CA:Asus Zenpad Z10 ZT500KL:Asus Zenpad Z8:Asus Zenpad Z8s ZT582KL:Huawei Enjoy Tablet 2:Huawei IDEOS S7:Huawei IDEOS S7 Slim:Huawei IDEOS S7 Slim CDMA:Huawei MatePad 10.4:Huawei MatePad 10.4 (2022):Huawei MatePad 10.8:Huawei MatePad 11 (2021):Huawei MatePad 5G:Huawei MatePad Pro 10.8 (2019):Huawei MatePad Pro 10.8 (2021):Huawei MatePad Pro 10.8 5G (2019):Huawei MatePad Pro 11 (2022):Huawei MatePad Pro 12.6 (2021):Huawei MatePad SE:Huawei MatePad T 10:Huawei MatePad T 10s:Huawei MatePad T8:Huawei MediaPad:Huawei MediaPad 10 FHD:Huawei MediaPad 10 Link:Huawei MediaPad 10 Link+:Huawei MediaPad 7 Lite:Huawei MediaPad 7 Vogue:Huawei MediaPad 7 Youth:Huawei MediaPad 7 Youth2:Huawei MediaPad M1:Huawei MediaPad M2 10.0:Huawei MediaPad M2 7.0:Huawei MediaPad M2 8.0:Huawei MediaPad M3 8.4:Huawei MediaPad M3 Lite 10:Huawei MediaPad M3 Lite 8:Huawei MediaPad M5 10:Huawei MediaPad M5 10 (Pro):Huawei MediaPad M5 8:Huawei MediaPad M5 lite:Huawei MediaPad M5 Lite 8:Huawei MediaPad M6 10.8:Huawei MediaPad M6 8.4:Huawei MediaPad M6 Turbo 8.4:Huawei MediaPad S7-301w:Huawei MediaPad T1 10:Huawei MediaPad T1 7.0:Huawei MediaPad T1 7.0 Plus:Huawei MediaPad T1 8.0:Huawei MediaPad T2 10.0 Pro:Huawei MediaPad T2 7.0:Huawei MediaPad T2 7.0 Pro:Huawei MediaPad T3 10:Huawei MediaPad T3 7.0:Huawei MediaPad T3 8.0:Huawei MediaPad T5:Huawei MediaPad X1:Huawei MediaPad X2:BlackBerry 4G LTE Playbook:BlackBerry 4G Playbook HSPA+:BlackBerry Playbook:BlackBerry Playbook 2012:BlackBerry Playbook Wimax:HP 10 Plus:HP 7 Plus:HP 8:HP Pro Slate 10 EE G1:HP Pro Slate 12:HP Pro Slate 8:HP Slate 17:HP Slate 7:HP Slate10 HD:HP Slate7 Extreme:HP Slate7 Plus:HP Slate8 Pro:HP TouchPad:HP TouchPad 4G:Google Pixel C:Sony Tablet P:Sony Tablet P 3G:Sony Tablet S:Sony Tablet S 3G:Sony Xperia Tablet S:Sony Xperia Tablet S 3G:Sony Xperia Tablet Z LTE:Sony Xperia Tablet Z Wi-Fi:Sony Xperia Z2 Tablet LTE:Sony Xperia Z2 Tablet Wi-Fi:Sony Xperia Z3 Tablet Compact:Sony Xperia Z4 Tablet LTE:Sony Xperia Z4 Tablet WiFi:Panasonic Toughpad FZ-A1:Panasonic Toughpad JT-B1:HTC Flyer:HTC Flyer Wi-Fi:HTC Jetstream:HTC Nexus 9:LG G Pad 10.1:LG G Pad 10.1 LTE:LG G Pad 5 10.1:LG G Pad 7.0:LG G Pad 7.0 LTE:LG G Pad 8.0:LG G Pad 8.0 LTE:LG G Pad 8.3:LG G Pad 8.3 LTE:LG G Pad II 10.1:LG G Pad II 8.0 LTE:LG G Pad II 8.3 LTE:LG G Pad III 10.1 FHD:LG G Pad III 8.0 FHD:LG G Pad IV 8.0 FHD:LG G Pad X 8.0:LG Optimus Pad LTE:LG Optimus Pad V900:LG Ultra Tab:Motorola DROID XYBOARD 10.1 MZ617:Motorola DROID XYBOARD 8.2 MZ609:Motorola Moto Tab G62:Motorola Moto Tab G70:Motorola Tab G20:Motorola XOOM 2 3G MZ616:Motorola XOOM 2 Media Edition 3G MZ608:Motorola XOOM 2 Media Edition MZ607:Motorola XOOM 2 MZ615:Motorola XOOM Media Edition MZ505:Motorola XOOM MZ600:Motorola XOOM MZ601:Motorola XOOM MZ604:ZTE Grand X View 2:ZTE Light Tab 2 V9A:ZTE Light Tab 3 V9S:ZTE Light Tab 300:ZTE Light Tab V9C:ZTE Optik:ZTE PF 100:ZTE T98:ZTE V81:ZTE V96:Acer Chromebook Tab 10:Acer Iconia A1-830:Acer Iconia B1-720:Acer Iconia B1-721:Acer Iconia One 7 B1-730:Acer Iconia One 8 B1-820:Acer Iconia Tab 10 A3-A30:Acer Iconia Tab 10 A3-A40:Acer Iconia Tab 7 A1-713:Acer Iconia Tab 7 A1-713HD:Acer Iconia Tab 8 A1-840FHD:Acer Iconia Tab A1-810:Acer Iconia Tab A1-811:Acer Iconia Tab A100:Acer Iconia Tab A101:Acer Iconia Tab A110:Acer Iconia Tab A200:Acer Iconia Tab A210:Acer Iconia Tab A3:Acer Iconia Tab A3-A20:Acer Iconia Tab A3-A20FHD:Acer Iconia Tab A500:Acer Iconia Tab A501:Acer Iconia Tab A510:Acer Iconia Tab A511:Acer Iconia Tab A700:Acer Iconia Tab A701:Acer Iconia Tab B1-710:Acer Iconia Tab B1-A71:Acer Predator 8:Lenovo A10-70 A7600:Lenovo A7-30 A3300:Lenovo A7-50 A3500:Lenovo A8-50 A5500:Lenovo IdeaPad A1:Lenovo IdeaPad K1:Lenovo IdeaPad S2:Lenovo IdeaTab A1000:Lenovo IdeaTab A2107:Lenovo IdeaTab A3000:Lenovo IdeaTab S6000:Lenovo IdeaTab S6000F:Lenovo IdeaTab S6000H:Lenovo IdeaTab S6000L:Lenovo LePad S2007:Lenovo LePad S2010:Lenovo M10 Plus:Lenovo Moto Tab:Lenovo Pad:Lenovo Pad Plus:Lenovo Pad Pro:Lenovo Pad Pro 2022:Lenovo S5000:Lenovo Tab 2 A10-70:Lenovo Tab 2 A7-10:Lenovo Tab 2 A7-20:Lenovo Tab 2 A7-30:Lenovo Tab 2 A8-50:Lenovo Tab 4 10:Lenovo Tab 4 10 Plus:Lenovo Tab 4 8:Lenovo Tab 4 8 Plus:Lenovo Tab 7:Lenovo Tab 7 Essential:Lenovo Tab K10:Lenovo Tab M10 HD Gen 2:Lenovo Tab M10 Plus (3rd Gen):Lenovo Tab M7:Lenovo Tab M7 (3rd Gen):Lenovo Tab M8 (3rd Gen):Lenovo Tab M8 (FHD):Lenovo Tab M8 (HD):Lenovo Tab P11:Lenovo Tab P11 5G:Lenovo Tab P11 Gen 2:Lenovo Tab P11 Plus:Lenovo Tab P11 Pro:Lenovo Tab P11 Pro Gen 2:Lenovo Tab P12 Pro:Lenovo Tab S8:Lenovo Tab V7:Lenovo Tab3 10:Lenovo Tab3 7:Lenovo Tab3 8:Lenovo Tab3 8 Plus:Lenovo Yoga Smart Tab:Lenovo Yoga Tab 11:Lenovo Yoga Tab 13:Lenovo Yoga Tab 3 10:Lenovo Yoga Tab 3 8.0:Lenovo Yoga Tab 3 Plus:Lenovo Yoga Tab 3 Pro:Lenovo Yoga Tablet 10:Lenovo Yoga Tablet 10 HD+:Lenovo Yoga Tablet 2 10.1:Lenovo Yoga Tablet 2 8.0:Lenovo Yoga Tablet 2 Pro:Lenovo Yoga Tablet 8:Alcatel 1T 10:Alcatel 1T 7:Alcatel 3T 10:Alcatel 3T 8:Alcatel Hero 8:Alcatel One Touch Evo 7:Alcatel One Touch Evo 7 HD:Alcatel One Touch Evo 8HD:Alcatel One Touch T10:Alcatel One Touch Tab 7:Alcatel One Touch Tab 7 HD:Alcatel One Touch Tab 8 HD:Alcatel Pixi 3 (10):Alcatel Pixi 3 (7):Alcatel Pixi 3 (7) 3G:Alcatel Pixi 3 (7) LTE:Alcatel Pixi 3 (8) 3G:Alcatel Pixi 3 (8) LTE:Alcatel Pixi 4 (7):Alcatel Pixi 7:Alcatel Pixi 8:Alcatel Pop 10:Alcatel Pop 7:Alcatel Pop 7 LTE:Alcatel Pop 7S:Alcatel Pop 8:Alcatel Pop 8S:Alcatel Smart Tab 7:Toshiba Excite 10 AT305:Toshiba Excite 10 SE:Toshiba Excite 13 AT335:Toshiba Excite 7.7 AT275:Toshiba Excite 7c AT7-B8:Toshiba Excite AT200:Toshiba Excite Go:Toshiba Excite Pro:Toshiba Excite Pure:Toshiba Excite Write:Blackview Tab 10:Blackview Tab 10 Pro:Blackview Tab 11:Blackview Tab 12:Blackview Tab 13:Blackview Tab 6:Blackview Tab 7:Blackview Tab 8:Blackview Tab 8E:Blackview Tab 9:Dell Streak 10 Pro:Dell Streak 7:Dell Streak 7 Wi-Fi:Dell Venue:Dell Venue 7:Dell Venue 7 8 GB:Dell Venue 8:Dell Venue 8 7000:Microsoft Surface:Microsoft Surface 2:Microsoft Surface Duo:Microsoft Surface Duo 2:Allview 2 Speed Quad:Allview 3 Speed Quad HD:Allview AX3 Party:Allview AX4 Nano:Allview AX4 Nano Plus:Allview AX501Q:Allview City Life:Allview City+:Allview Viva 1003G:Allview Viva 1003G Lite:Allview Viva 803G:Allview Viva C701:Allview Viva C703:Allview Viva D8:Allview Viva H10 HD:Allview Viva H10 LTE:Allview Viva H1001 LTE:Allview Viva H7 LTE:Allview Viva H7S:Allview Viva H8:Allview Viva H8 LTE:Allview Viva Home:Allview Viva i10G:Allview Viva i8:Allview Viva Q7 Life:Allview Viva Q8:Allview Wi10N PRO:Allview Wi7:Allview Wi8G:Amazon Fire 7:Amazon Fire 7 (2017):Amazon Fire HD 10:Amazon Fire HD 10 (2017):Amazon Fire HD 10 (2019):Amazon Fire HD 10 (2021):Amazon Fire HD 10 Plus (2021):Amazon Fire HD 6:Amazon Fire HD 7:Amazon Fire HD 8:Amazon Fire HD 8 (2017):Amazon Fire HD 8 (2020):Amazon Fire HD 8 Plus (2020):Amazon Fire HDX 8.9 (2014):Amazon Kindle Fire:Amazon Kindle Fire HD:Amazon Kindle Fire HD (2013):Amazon Kindle Fire HD 8.9:Amazon Kindle Fire HD 8.9 LTE:Amazon Kindle Fire HDX:Amazon Kindle Fire HDX 8.9:Archos Diamond Tab:BLU Life View 8.0:BLU Life View Tab:BLU M7L:BLU M8L:BLU M8L 2022:BLU M8L Plus:BLU Touch Book 7.0:BLU Touch Book 7.0 Lite:BLU Touch Book 7.0 Plus:BLU Touch Book 9.7:BLU Touch Book M7:BLU Touchbook G7:BLU Touchbook M7:BLU Touchbook M7 Pro:BQ Aquaris M10:Celkon C720:Celkon C820:Celkon CT 1:Celkon CT 2:Celkon CT 7:Celkon CT 9:Celkon CT-888:Celkon CT-910:Celkon CT-910+:Honor Pad 2:Honor Pad 5 10.1:Honor Pad 5 8:Honor Pad 6:Honor Pad 8:Honor Pad X6:Honor Pad X8:Honor Pad X8 Lite:Honor Tab 5:Honor Tab 7:Honor Tablet V7:Honor Tablet V7 Pro:Honor Tablet X7:Honor V6:Icemobile G10:Icemobile G2:Icemobile G3:Icemobile G5:Icemobile G7:Icemobile G7 Pro:Icemobile G8:Icemobile G8 LTE:Jolla Tablet:Karbonn A34:Karbonn A37:Karbonn Smart Tab 10:Karbonn Smart Tab 7:Karbonn Smart Tab 8:Karbonn Smart Tab 9:Karbonn Smart Tab2:Maxwest Tab Phone 72DC:Micromax Canvas Tab P470:Micromax Canvas Tab P650:Micromax Canvas Tab P666:Micromax Canvas Tab P690:Micromax Funbook 3G P560:Micromax Funbook 3G P600:Micromax Funbook Alfa P250:Micromax Funbook Infinity P275:Micromax Funbook P300:Micromax Funbook Pro:Micromax Funbook Talk P360:Micromax Funbook Talk P362:Nvidia Shield:Nvidia Shield K1:Nvidia Shield LTE:Pantech Element:Plum Debut:Plum Link:Plum Link II:Plum Link Plus:Plum Optimax 10:Plum Optimax 11:Plum Optimax 12:Plum Optimax 13:Plum Optimax 2:Plum Optimax 7.0:Plum Optimax 8.0:Plum Ten 3G:Plum Z708:Plum Z710:Posh Equal Lite W700:Prestigio MultiPad 10.1 Ultimate:Prestigio MultiPad 10.1 Ultimate 3G:Prestigio MultiPad 2 Prime Duo 8.0:Prestigio MultiPad 2 Pro Duo 8.0 3G:Prestigio MultiPad 2 Ultra Duo 8.0:Prestigio MultiPad 2 Ultra Duo 8.0 3G:Prestigio Multipad 4 Quantum 10.1:Prestigio MultiPad 4 Quantum 10.1 3G:Prestigio Multipad 4 Quantum 7.85:Prestigio Multipad 4 Quantum 9.7:Prestigio MultiPad 4 Quantum 9.7 Colombia:Prestigio MultiPad 4 Ultimate 8.0 3G:Prestigio MultiPad 4 Ultra Quad 8.0 3G:Prestigio MultiPad 7.0 HD:Prestigio MultiPad 7.0 HD +:Prestigio MultiPad 7.0 Prime:Prestigio MultiPad 7.0 Prime +:Prestigio MultiPad 7.0 Prime 3G:Prestigio MultiPad 7.0 Prime Duo:Prestigio MultiPad 7.0 Prime Duo 3G:Prestigio MultiPad 7.0 Pro:Prestigio MultiPad 7.0 Pro Duo:Prestigio MultiPad 7.0 Ultra:Prestigio MultiPad 7.0 Ultra +:Prestigio MultiPad 7.0 Ultra + New:Prestigio MultiPad 7.0 Ultra Duo:Prestigio MultiPad 8.0 HD:Prestigio MultiPad 8.0 Pro Duo:Prestigio MultiPad 8.0 Ultra Duo:Prestigio MultiPad 9.7 Ultra Duo:Prestigio MultiPad Note 8.0 3G:QMobile QTab V10:QMobile QTab X50:Spice Mi-1010 Stellar Pad:Spice Mi-720:Spice Mi-725 Stellar Slatepad:T-Mobile G-Slate:T-Mobile SpringBoard:TCL 10 TabMax:TCL 10 TabMid:TCL NxtPaper:TCL NxtPaper 10s:TCL Tab 10 HD 4G:TCL Tab 10L:TCL Tab 10s:TCL Tab 10s 5G:TCL Tab 8 4G:Ulefone Tab A7:verykool Kolorpad LTE TL8010:verykool R800:verykool T742:verykool T7440 Kolorpad II:verykool T7445:Vodafone Smart Tab 10:Vodafone Smart Tab 4G:Vodafone Smart Tab 7:Vodafone Smart Tab II 10:Vodafone Smart Tab II 7:Vodafone Smart Tab III 10.1:Vodafone Smart Tab III 7:Vodafone Smart Tab N8:Vodafone Tab Prime 6:Vodafone Tab Prime 7:XOLO Play Tab 7.0:XOLO Play Tegra Note:XOLO Tab:Yezz EPIC 3:Yezz Epic T7:Yezz Epic T7ED:Yezz Epic T7FD:Lenovo Legion Y700:Lenovo M10 FHD REL";

            ProductAttributesHelper.ConvertListToHtmlFile(listInput, _filePath);
        }
        else if (_command == Command.FindMatchingKeys)
        {
            foreach (var category in Constants.TIKTOKCATEGORIES)
            {
                var productAttributes = _client.SendRequest<ProductAttributes>(HttpMethod.Get,
                    "/api/products/attributes", null, "Failed to get category rule", null,
                    new Dictionary<string, string> { { "category_id", category.Value } });

                if (productAttributes.IsSuccess && productAttributes?.Data?.Attributes != null)
                {
                    Console.WriteLine("Category: {0}, Contains Fields: {1}, Matching Keys: {2}", category.Key,
                        DictionaryHelper.AreAllKeysPresent(productAttributes.Data.Attributes, Constants.REQUIREDFIELDS), DictionaryHelper.GetMatchingKeys(productAttributes.Data.Attributes, Constants.REQUIREDFIELDS));
                    Console.WriteLine("\n");
                }
            }
        }
        else if (_command == Command.BuildCategoryBreadCrumbs)
        {
            var tikTokCategories = _client.SendRequest<TikTokCategories>(HttpMethod.Get,
                        "/api/products/categories", null, "Failed to get categories");

            if (tikTokCategories.IsSuccess && tikTokCategories?.Data?.CategoryList != null)
            {
                var generator = new CategoryBreadcrumbsGenerator();
                var breadcrumbs = generator.GenerateBreadcrumbs(tikTokCategories.Data.CategoryList);
                CSVWriter.WriteBreadCrumbsToCsv(breadcrumbs, _filePath);
            }
        }
        else if (_command == Command.GenerateFieldDescriptions)
        {
            var tikTokCategories = _client.SendRequest<TikTokCategories>(HttpMethod.Get,
                       "/api/products/categories", null, "Failed to get categories");

            if (tikTokCategories.IsSuccess && tikTokCategories?.Data?.CategoryList != null)
            {
                PopulateProductAttributes(tikTokCategories);

                ProductAttributesHelper.GenerateFieldDescriptions(_attributes, true);
            }
        }
        else if (_command == Command.DeleteDuplicateProducts)
        {
            // Get basic product data
            var tiktokProducts = TikTokProductImport.GetBasicProductData(_client);

            if (tiktokProducts != null && tiktokProducts.Any())
            {
                // Group products by the Name property
                var duplicateGroups = tiktokProducts.GroupBy(product => product.Name);

                // Filter groups where the number of SKUs and SellerSkus match
                var duplicateProducts = duplicateGroups
                    .Where(group => group.All(product =>
                        product.Skus.Count == duplicateGroups.First(g => g.Key == group.Key).First().Skus.Count &&
                        product.Skus.Any(sku =>
                            group.First().Skus.Any(s => s.SellerSku == sku.SellerSku))))
                    .SelectMany(group => group)
                    .ToList();

                // Create a list to keep one product from each duplicate set
                var productsToKeep = new List<GetTikTokProduct>();

                foreach (var group in duplicateGroups)
                {
                    // Find the product to keep (e.g., the first one in the group) and add it to productsToKeep
                    productsToKeep.Add(group.First());
                }

                // Remove the products to keep from the duplicateProducts list; i.e there are 4 duplicates -> delete 3 -> keep 1
                duplicateProducts.RemoveAll(product => productsToKeep.Contains(product));

                // Delete duplicate products
                if (duplicateProducts != null && duplicateProducts.Any())
                {
                    // Get product ids only
                    var productIds = duplicateProducts.Select(product => product.Id).ToList();

                    // Split productIdsToBeDeleted into groups of 20 or less
                    var groupedProductIds = new List<List<string>>();
                    for (int i = 0; i < productIds.Count; i += 20)
                    {
                        groupedProductIds.Add(productIds.Skip(i).Take(20).ToList());
                    }

                    foreach (var group in groupedProductIds)
                    {
                        var jsonBody = new
                        {
                            product_ids = group
                        };

                        var response = _client.SendRequest<DeleteProducts>(HttpMethod.Delete, "/api/products",
                            JsonConvert.SerializeObject(jsonBody), "");
                    }
                }


            }
            else { throw new Exception("Invalid command"); }
        }
    }

    public static void PopulateProductAttributes(TikTokCategories tikTokCategories)
    {
        foreach (var category in tikTokCategories.Data.CategoryList)
        {
            var productAttributes = _client.SendRequest<ProductAttributes>(HttpMethod.Get,
               "/api/products/attributes", null, "Failed to get category rule", null,
               new Dictionary<string, string> { { "category_id", category.Id.ToString() } });

            if (productAttributes.IsSuccess && productAttributes?.Data?.Attributes != null)
            {
                foreach (var attribute in productAttributes.Data.Attributes)
                {
                    attribute.CategoryName = category.LocalDisplayName.ToString();
                    attribute.CategoryId = category.Id.ToString();
                    attribute.ConcatenatedValues = "";

                    if (attribute.Values != null && attribute.Values.Count > 0)
                    {
                        List<string> valueList = attribute.Values.Select(value => CSVWriter.EscapeCsvField(value.Name)).ToList();
                        attribute.ConcatenatedValues = string.Join(":", valueList);
                    }
                }
                _attributes = _attributes.Concat(productAttributes.Data.Attributes).ToList();
            }
        }
    }

    private static void GenerateCategoryReport(TikTokCategories tikTokCategories)
    {
        PopulateProductAttributes(tikTokCategories);

        var groups = _attributes.GroupBy(o => o.CategoryName);
        string textFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "report.txt");

        using (var writer = new StreamWriter(textFilePath))
        {
            writer.WriteLine($"Category Count: {tikTokCategories.Data.CategoryList.Count}");
            writer.WriteLine($"Field Count: {_attributes.Count}");
            writer.WriteLine($"Categories With Properties: {groups.Count()}");
            writer.WriteLine($"Categories With Required Properties: {groups.Count(group => group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory))}");
            writer.WriteLine($"Required Properties: {_attributes.Where(x => x?.InputType?.IsMandatory != null && x.InputType.IsMandatory).Count()}");
            foreach (var group in groups)
            {
                writer.WriteLine("Category: {0}, Count: {1}, Required: {2}", group.Key, group.Count(), group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory));
            }
        }

        Console.Clear();
        Console.WriteLine($"Category Count: {tikTokCategories.Data.CategoryList.Count}");
        Console.WriteLine($"Field Count: {_attributes.Count}");
        Console.WriteLine($"Categories With Properties: {groups.Count()}");
        Console.WriteLine($"Categories With Required Properties: {groups.Count(group => group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory))}");
        Console.WriteLine($"Required Properties: {_attributes.Where(x => x?.InputType?.IsMandatory != null && x.InputType.IsMandatory).Count()}");

        foreach (var group in groups)
        {
            Console.WriteLine("Category: {0}, Count: {1}, Required: {2}", group.Key, group.Count(), group.Any(item => item?.InputType?.IsMandatory != null && item.InputType.IsMandatory));
        }

        //Full Attributes List
        CSVWriter.WriteCsvFile(_attributes, "product_attributes.csv");

        //Filtered for unique attributes that are required
        CSVWriter.WriteCsvFile(_attributes
                    .Where(item => item?.InputType?.IsMandatory == true && ((item?.Values != null && item.Values.Any()) || (item?.Values == null || !item.Values.Any())))
                    .GroupBy(item => item.Name) // Group by item.Name
                    .Select(group => group.First()) // Select the first item from each group
                    .ToList(), "product_attributes_unique.csv");
    }
}