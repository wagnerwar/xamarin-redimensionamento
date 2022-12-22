using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading;
namespace Redimensionamento.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private INavigation _navigation { get; set; }
        public ICommand EnviarArquivoCommand { get; set; }
        private String _nomeArquivo;
        public String NomeArquivo
        {
            get { return _nomeArquivo; }
            set
            {
                _nomeArquivo = value;
                OnPropertyChanged();
            }
        }
        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }
        private String _resolucao;
        public String Resolucao
        {
            get { return _resolucao; }
            set
            {
                _resolucao = value;
                OnPropertyChanged();
            }
        }
        private byte[] _arquivo;
        public byte[] Arquivo
        {
            get { return _arquivo; }
            set
            {
                _arquivo = value;
                OnPropertyChanged();
            }
        }
        private byte[] _arquivoRedimensionado;
        public byte[] ArquivoRedimensionado
        {
            get { return _arquivoRedimensionado; }
            set
            {
                _arquivoRedimensionado = value;
                OnPropertyChanged();
            }
        }
        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            EnviarArquivoCommand = new Command(async () => await EnviarArquivo());
            IsLoading = false;
            GroupName = "Grupo";
        }
        public async Task EnviarArquivo()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(TimeSpan.FromSeconds(3));
                IsLoading = false;
                if (String.IsNullOrEmpty(Resolucao))
                {
                    throw new Exception("Resolução deve ser selecionada");
                }
                throw new Exception("Teste");
                var photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Erro", ex.Message);
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            try
            {
                // canceled
                if (photo == null)
                {
                    //PhotoPath = null;
                    return;
                }
                // recuperar binário e salvar no banco
                Arquivo = File.ReadAllBytes(photo.FullPath);
                NomeArquivo = photo.FileName;
                // Processamento
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Sucesso", "Arquivo enviado com sucesso");
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Erro", ex.Message);
            }            
        }
    }
}
