using System.Collections.Generic;
using UnityEngine;

public class NameGenerator
{
    static List<string> CanidPrefixes = new List<string>
    {
        "Agr",
        "Aurel",
        "Caes",
        "Claudi",
        "Corn",
        "Dom",
        "Flav",
        "Gai",
        "Juli",
        "Luc",
        "Mar",
        "Max",
        "Ner",
        "Pum",
        "Serv",
        "Sulpic",
        "Tib",
        "Tull",
        "Val",
        "Vit",
        "Vir"
    };

    static List<string> CanidSuffixes = new List<string>
    {
        "amus",
        "ellus",
        "enus",
        "ensis",
        "erius",
        "ianus",
        "illus",
        "inus",
        "ius",
        "or",
        "tor",
        "tus",
        "ulus",
        "us"
    };

    static List<string> FelisFirstNamePrefixes = new List<string>
    {
        "A", "Ba", "Be", "Bi", "Bo", "Bu",
        "Ca", "Ce", "Ci", "Co", "Cu",
        "Da", "De", "Di", "Do", "Du",
        "Fa", "Fe", "Fi", "Fo", "Fu",
        "Ga", "Ge", "Gi", "Go", "Gu",
        "Ha", "He", "Hi", "Ho", "Hu",
        "Ja", "Je", "Ji", "Jo", "Ju",
        "Ka", "Ke", "Ki", "Ko", "Ku",
        "La", "Le", "Li", "Lo", "Lu",
        "Ma", "Me", "Mi", "Mo", "Mu",
        "Na", "Ne", "Ni", "No", "Nu",
        "Pa", "Pe", "Pi", "Po", "Pu",
        "Ra", "Re", "Ri", "Ro", "Ru",
        "Sa", "Se", "Si", "So", "Su",
        "Ta", "Te", "Ti", "To", "Tu",
        "Va", "Ve", "Vi", "Vo", "Vu",
        "Wa", "We", "Wi", "Wo", "Wu",
        "Ya", "Ye", "Yi", "Yo", "Yu",
        "Za", "Ze", "Zi", "Zo", "Zu"
    };

    static List<string> FelisFirstNameCenter = new List<string>
    {
        "bal", "bam", "ban", "bar", "bas", "baz",
        "cal", "cam", "can", "car", "cas", "caz",
        "dal", "dam", "dan", "dar", "das", "daz",
        "fal", "fam", "fan", "far", "fas", "faz",
        "gal", "gam", "gan", "gar", "gas", "gaz",
        "hal", "ham", "han", "har", "has", "haz",
        "jal", "jam", "jan", "jar", "jas", "jaz",
        "kal", "kam", "kan", "kar", "kas", "kaz",
        "lal", "lam", "lan", "lar", "las", "laz",
        "mal", "mam", "man", "mar", "mas", "maz",
        "nal", "nam", "nan", "nar", "nas", "naz",
        "pal", "pam", "pan", "par", "pas", "paz",
        "ral", "ram", "ran", "rar", "ras", "raz",
        "sal", "sam", "san", "sar", "sas", "saz",
        "tal", "tam", "tan", "tar", "tas", "taz",
        "val", "vam", "van", "var", "vas", "vaz",
        "wal", "wam", "wan", "war", "was", "waz",
        "yal", "yam", "yan", "yar", "yas", "yaz",
        "zal", "zam", "zan", "zar", "zas", "zaz"
    };

    static List<string> FelisFirstNameSuffixes = new List<string>
    {
        "ana", "andra", "ara", "asa", "aza",
        "ena", "endra", "era", "esa", "eza",
        "ina", "indra", "ira", "isa", "iza",
        "ona", "ondra", "ora", "osa", "oza"
    };

    static List<string> FelisLastNamePrefixes = new List<string>()
    {
        "Ash",
        "Aura",
        "Blaze",
        "Dawn",
        "Earth",
        "Ember",
        "Ether",
        "Fire",
        "Frost",
        "Light",
        "Mist",
        "Moon",
        "Nova",
        "Rift",
        "Sea",
        "Shade",
        "Sky",
        "Soul",
        "Star",
        "Sun",
        "Tide",
        "Wind"
    };

    static List<string> felisLastNameSuffixes = new List<string>
    {
        "bender",
        "binder",
        "breather",
        "caller",
        "dancer",
        "eater",
        "keeper",
        "mender",
        "rider",
        "seer",
        "seeker",
        "shaper",
        "shifter",
        "singer",
        "walker",
        "watcher",
        "weaver"

    };

    static List<string> mouseFolkFirstNamePrefixes = new List<string>()
    {
        "A", "Ba", "Be", "Bi", "Bo", "Bu",
        "Ca", "Ce", "Ci", "Co", "Cu",
        "Da", "De", "Di", "Do", "Du",
        "Fa", "Fe", "Fi", "Fo", "Fu",
        "Ga", "Ge", "Gi", "Go", "Gu",
        "Ha", "He", "Hi", "Ho", "Hu",
        "Ja", "Je", "Ji", "Jo", "Ju",
        "Ka", "Ke", "Ki", "Ko", "Ku",
        "La", "Le", "Li", "Lo", "Lu",
        "Ma", "Me", "Mi", "Mo", "Mu",
        "Na", "Ne", "Ni", "No", "Nu",
        "Pa", "Pe", "Pi", "Po", "Pu",
        "Ra", "Re", "Ri", "Ro", "Ru",
        "Sa", "Se", "Si", "So", "Su",
        "Ta", "Te", "Ti", "To", "Tu",
        "Va", "Ve", "Vi", "Vo", "Vu",
        "Wa", "We", "Wi", "Wo", "Wu",
        "Ya", "Ye", "Yi", "Yo", "Yu",
        "Za", "Ze", "Zi", "Zo", "Zu"
    };

    static List<string> mouseFolkFirstNameSuffixes = new List<string>
    {
        "ka", "ki",
        "beeka", "beeki",
        "deeka", "deeki",
        "keeka", "keeki",
        "meeka", "meeki",
        "neeka", "neeki",
        "peeka", "peeki",
        "reeka", "reeki",
        "teeka", "teeki",
        "veeka", "veeki",
        "zeeka", "zeeki"
    };

    public static string GenerateCanidName()
    {
        string firstName = GenerateName(CanidPrefixes, CanidSuffixes);
        string lastName = GenerateName(CanidPrefixes, CanidSuffixes);
        return $"{firstName} {lastName}";
    }

    public static string GenerateFelisName()
    {
        string firstName = GenerateName(FelisFirstNamePrefixes, FelisFirstNameCenter, FelisFirstNameSuffixes);
        string lastName = GenerateName(FelisLastNamePrefixes, felisLastNameSuffixes);
        return $"{firstName} {lastName}";
    }

    public static string GenerateMouseFolkName()
    {
        return GenerateName(mouseFolkFirstNamePrefixes, mouseFolkFirstNameSuffixes);
    }

    public static string GenerateName(List<string> prefix, List<string> suffix)
    {
        int prefixIndex = Random.Range(0, prefix.Count);
        int suffixIndex = Random.Range(0, suffix.Count);
        return prefix[prefixIndex] + suffix[suffixIndex];
    }

    public static string GenerateName(List<string> prefix, List<string> center, List<string> suffix)
    {
        int prefixIndex = Random.Range(0, prefix.Count);
        int centerIndex = Random.Range(0, center.Count);
        int suffixIndex = Random.Range(0, suffix.Count);
        return prefix[prefixIndex] + center[centerIndex] + suffix[suffixIndex];
    }
}