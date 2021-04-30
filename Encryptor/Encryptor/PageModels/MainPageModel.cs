using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using PropertyChanged;

namespace Encryptor.PageModels
{

    [AddINotifyPropertyChangedInterface]
    public class MainPageModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public string LabelBegin
        {
            get => labelBegin;
            set
            {
                labelBegin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelBegin)));
            }
        }
        private string labelBegin;
        public string EditorText
        {
            get => editorText;
            set
            {
                if (editorText != value)
                {
                    editorText = value;
                    LabelBegin = "";
                    LabelTitel = "";
                    LabelText = "";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditorText)));
                }
            }
        }
        private string editorText;
        public string KeyEntry { get; set; } = "Скорпион";
        public string FileNameEntry { get; set; }

        public string LabelTitel
        {
            get => labelTitel;
            set
            {
                labelTitel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelTitel)));
            }
        }
        private string labelTitel;
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LabelText)));
            }
        }
        private string labelText;

        public MainPageModel()
        {
            string text = "бщцфаирщри, бл ячъбиуъ щбюэсяёш гфуаа!!! \n" + 
                "у ъящэячэц ъэюоык, едщ бдв саэацкшгнбяр гчеа кчфцшубп цу ьгщпя вщвсящ, эвэчрысй юяуъщнщхо шпуъликугбз чъцшья с цощъвчщ ъфмес ю лгюлэ ёъяяр!" +
                " с моыящш шпмоец щаярдш цяэубфъ аьгэотызуа дщ, щръ кй юцкъщчьуац уыхэцэ ясч юбюяуяг ыовзсгюамщщ.внютвж тхыч эядкъябе цн юкъль," +
                " мэсццогл шяьфыоэьь ть эщсщжнашанэ ыюцен, уёюяыцчан мах гъъьуун шпмоыъй ч яяьпщъхэтпык яущм бпйэае!" +
                "чэьюмуд, оээ скфч саьбрвчёыа эядуцйт ъ уьгфщуяяёу фси а эацэтшцэч юпапёи, ьь уъубфмч ысь хффы ужц чьяцнааущ эгъщйаъф, ч п эиттпьк ярвчг гмубзньцы!" +
                "щб ьшяо шачюрэсч FirstLineSoftware ц ешчтфщацдпбр шыыь, р ыоф ячцсвкрщве бттй а ядсецсцкюкх эшашёрэсуъ якжще увюгщр в# уфн ысвчюпжзцж!" +
                " чй ёюычъ бщххыибй еьюхечр п хкъмэншёцч юятщвфцшчщ с хчю ъэ ч аачсюсчыщачрняун в шъюьэжцясиьццч агфуо ацаьяычсцы .Net, чэбф ыуюбпьщо с чыдпяхбцйг щктрж!";
            string path = "Result_v5.txt";
            File.WriteAllText(Path.Combine(folderPath, path), text);
        }

        public Command OnButtonClickedOpen => new Command(async () =>
        {
            var files = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
            if(files == null || files.Count() == 0)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", $"В папке {folderPath} нет файлов", "ОK");
                return;
            }

            var file = await App.Current.MainPage.DisplayActionSheet("Выберите файл", "Отмена", null, files.ToArray());

            if(file == "Отмена")
            {
                return;
            }            

            EditorText = File.ReadAllText(Path.Combine(folderPath, file));// Сделать через стрим
            LabelBegin =  $"Текст из файла {file}:";
            LabelTitel = "";
            LabelText = "";

        });

        public Command OnButtonClickedDel => new Command(async () =>
        {
            var files = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
            if (files == null || files.Count() == 0)
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", $"В папке {folderPath} нет файлов", "ОK");
                return;
            }

            var file = await App.Current.MainPage.DisplayActionSheet("Выберите файл", "Отмена", null, files.ToArray());

            if (file == "Отмена")
            {
                return;
            }

            File.Delete(Path.Combine(folderPath, file));
            LabelTitel = "";
            LabelText = "";
            EditorText = "";
            await App.Current.MainPage.DisplayAlert("Уведомление", "Файл удалён", "ОK");

        });

        public Command OnButtonClickedSeve => new Command(async () =>
        {
            if (String.IsNullOrEmpty(FileNameEntry))
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Не указано имя файла введите его в соответствующее поле", "ОK");
                return;
            }

            if (String.IsNullOrEmpty(LabelText))
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Нет сохраняемой информации, сначала нужно зашифровать или расшифровать текст  ", "ОK");
                return;
            }
            if (File.Exists(Path.Combine(folderPath, FileNameEntry)))
            {
                bool isRewrited = await App.Current.MainPage.DisplayAlert("Подтверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");
                if (isRewrited == false) return;
            }
            else
            {
                bool isRewrited = await App.Current.MainPage.DisplayAlert("Подтверждение", $"Сохранить файл в {folderPath}", "Да", "Нет");
                if (isRewrited == false) return;
            }
            File.WriteAllText(Path.Combine(folderPath, FileNameEntry), LabelText);
            LabelTitel = "";
            LabelText = "";
            EditorText = "";
            await App.Current.MainPage.DisplayAlert("Уведомление", "Текст сохранён", "ОK");

        });

        public Command OnButtonClicked => new Command(async() =>
        {
            if (String.IsNullOrEmpty(KeyEntry) || String.IsNullOrEmpty(EditorText))
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Текст или ключ не задан", "ОK");
                return;
            }

            LabelText = Cipher.Encrypt(EditorText, KeyEntry, true);
            if(LabelText == null)
            {
                LabelText = "";
                LabelTitel = "";
                await App.Current.MainPage.DisplayAlert("Уведомление", "Некорректный ключ", "ОK");
                return;
            }
            LabelTitel = "Зашифрованный текст:";
            
        });

        public Command OnButtonClicked2 => new Command(async() =>
        {
            if (String.IsNullOrEmpty(EditorText) || String.IsNullOrEmpty(KeyEntry))
            {
                await App.Current.MainPage.DisplayAlert("Уведомление", "Текст или ключ не задан", "ОK");
                return;
            }

            LabelText = Cipher.Encrypt(EditorText, KeyEntry, false);
            if (LabelText == null)
            {
                LabelText = "";
                LabelTitel = "";
                await App.Current.MainPage.DisplayAlert("Уведомление", "Некорректный ключ", "ОK");
                return;
            }
            LabelTitel = "Расшифрованный текст:";
            
        });



    }
}
