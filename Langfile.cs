using System.Globalization;

namespace Edge_Updater
{
    public partial class Langfile
    {
        public static string Texts(string langText)
        {
            CultureInfo culture1 = CultureInfo.CurrentUICulture;
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    if (langText == "Button10")
                    {
                        return "Выход";
                    }
                    else if (langText == "Button9")
                    {
                        return "Установить все";
                    }
                    else if (langText == "Button9UAll")
                    {
                        return "Обновить все";
                    }
                    else if (langText == "Label10")
                    {
                        return "Установить все версии x86 и/или x64";
                    }
                    else if (langText == "checkBox4")
                    {
                        return "Игнорировать проверку версии";
                    }
                    else if (langText == "checkBox1")
                    {
                        return "Разные версии в отдельных папках";
                    }
                    else if (langText == "checkBox5")
                    {
                        return "Создать ярлык на рабочем столе";
                    }
                    else if (langText == "downUnpstart")
                    {
                        return "Распаковка";
                    }
                    else if (langText == "downUnpfine")
                    {
                        return "Распакованный";
                    }
                    else if (langText == "infoLabel")
                    {
                        return "Доступна новая версия";
                    }
                    else if (langText == "laterButton")
                    {
                        return "нет";
                    }
                    else if (langText == "updateButton")
                    {
                        return "Да";
                    }
                    else if (langText == "downLabel")
                    {
                        return "ОБНОВИТЬ";
                    }
                    else if (langText == "MeassageVersion")
                    {
                        return "Данная версия уже установлена";
                    }
                    else if (langText == "MeassageRunning")
                    {
                        return "Необходимо закрыть Microsoft Edge (Chromium) перед обновлением.";
                    }
                    else if (langText == "Register")
                    {
                        return "регистр";
                    }
                    else if (langText == "Remove")
                    {
                        return "Удалить";
                    }
                    else if (langText == "Edge")
                    {
                        return " как браузер по умолчанию";
                    }
                    else if (langText == "Extra")
                    {
                        return "отде́льно";
                    }
                    else if (langText == "VInfo")
                    {
                        return "О версиях";
                    }
                    break;
                case "de":
                    if (langText == "Button10")
                    {
                        return "Beenden";
                    }
                    else if (langText == "Button9")
                    {
                        return "Alle Installieren";
                    }
                    else if (langText == "Button9UAll")
                    {
                        return "Alle Updaten";
                    }
                    else if (langText == "Label10")
                    {
                        return "Alle x86 und oder x64 installieren";
                    }
                    else if (langText == "checkBox4")
                    {
                        return "Versionkontrolle ignorieren";
                    }
                    else if (langText == "checkBox1")
                    {
                        return "Für jede Version einen eigenen Ordner";
                    }
                    else if (langText == "checkBox5")
                    {
                        return "Eine Verknüpfung auf dem Desktop erstellen";
                    }
                    else if (langText == "downUnpstart")
                    {
                        return "Entpacken";
                    }
                    else if (langText == "downUnpfine")
                    {
                        return "Entpackt";
                    }
                    else if (langText == "infoLabel")
                    {
                        return "Eine neue Version ist verfügbar";
                    }
                    else if (langText == "laterButton")
                    {
                        return "Nein";
                    }
                    else if (langText == "updateButton")
                    {
                        return "Ja";
                    }
                    else if (langText == "downLabel")
                    {
                        return "Update now";
                    }
                    else if (langText == "MeassageVersion")
                    {
                        return "Die selbe Version ist bereits installiert";
                    }
                    else if (langText == "MeassageRunning")
                    {
                        return "Bitte schließen Sie den laufenden Microsoft Edge (Chromium), bevor Sie den Browser aktualisieren.";
                    }
                    else if (langText == "Register")
                    {
                        return "Registrieren";
                    }
                    else if (langText == "Remove")
                    {
                        return "Entfernen";
                    }
                    else if (langText == "Edge")
                    {
                        return " als Standardbrowser";
                    }
                    else if (langText == "Extra")
                    {
                        return "Extras";
                    }
                    else if (langText == "VInfo")
                    {
                        return "Versions Info";
                    }
                    break;
                default:
                    if (langText == "Button10")
                    {
                        return "Quit";
                    }
                    else if (langText == "Button9")
                    {
                        return "Install all";
                    }
                    else if (langText == "Button9UAll")
                    {
                        return "Update all";
                    }
                    else if (langText == "Label10")
                    {
                        return "Install all x86 and or x64";
                    }
                    else if (langText == "checkBox4")
                    {
                        return "Ignore version check";
                    }
                    else if (langText == "checkBox1")
                    {
                        return "Create a Folder for each version";
                    }
                    else if (langText == "checkBox5")
                    {
                        return "Create a shortcut on the desktop";
                    }
                    else if (langText == "downUnpstart")
                    {
                        return "Unpacking";
                    }
                    else if (langText == "downUnpfine")
                    {
                        return "Unpacked";
                    }
                    else if (langText == "infoLabel")
                    {
                        return "A new version is available";
                    }
                    else if (langText == "laterButton")
                    {
                        return "No";
                    }
                    else if (langText == "updateButton")
                    {
                        return "Yes";
                    }
                    else if (langText == "downLabel")
                    {
                        return "Update now";
                    }
                    else if (langText == "MeassageVersion")
                    {
                        return "The same version is already installed";
                    }
                    else if (langText == "MeassageRunning")
                    {
                        return "Please close the running Microsoft Edge (Chromium) before updating the browser.";
                    }
                    else if (langText == "Register")
                    {
                        return "Register";
                    }
                    else if (langText == "Remove")
                    {
                        return "Remove";
                    }
                    else if (langText == "Edge")
                    {
                        return " as default browser";
                    }
                    else if (langText == "Extra")
                    {
                        return "Extras";
                    }
                    else if (langText == "VInfo")
                    {
                        return "Version Info";
                    }
                    break;
            }
            return "";
        }
    }
}
