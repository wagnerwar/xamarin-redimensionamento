using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using Redimensionamento.Interface;
namespace Redimensionamento.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private String Pasta = "redimensionados";
        private INavigation _navigation { get; set; }
        public ICommand EnviarArquivoCommand { get; set; }
        public ICommand LimparDiretorioCommand { get; set; }
        public ICommand PlotarLogoCommand { get; set; }
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
            LimparDiretorioCommand = new Command(async () => await LimparDiretorio());
            IsLoading = false;
            GroupName = "Grupo";
        }
        public async Task EnviarArquivo()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(TimeSpan.FromSeconds(1));                
                if (String.IsNullOrEmpty(Resolucao))
                {
                    throw new Exception("Resolução deve ser selecionada");
                }
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
                String tipoConteudo = photo.ContentType;
                String modeloTipo = @"^image.*$";
                Match match = Regex.Match(tipoConteudo, modeloTipo);                
                if (match.Success)
                {
                    ArquivoRedimensionado = Arquivo;
                    // Verifica duplicidade
                    bool retorno = await VerificarExistenciaArquivo(NomeArquivo);
                    if(retorno == false)
                    {
                        throw new Exception("Já existe arquivo com este nome na pasta");
                    }
                    // Redimensionamento
                    await RedimensionarArquivo();
                    // Envio do arquivo para uma pasta do aparelho
                    await UploadArquivo();
                    IsLoading = false;
                    MessagingCenter.Send<MainPage, String>(new MainPage(), "Sucesso", "Arquivo enviado com sucesso");
                }
                else
                {
                    IsLoading = false;
                    throw new Exception("Arquivo inválido");
                }                
            }
            catch(Exception ex)
            {
                IsLoading = false;
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Erro", ex.Message);
            }            
        }
        async Task UploadArquivo()
        {            
            String caminho = GetCaminhoPasta();
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            String caminhoArquivo = Path.Combine(caminho, NomeArquivo);
            File.WriteAllBytes(caminhoArquivo, ArquivoRedimensionado);
        }
        async Task RedimensionarArquivo()
        {
            try
            {
                String[] valoresResolucao = Resolucao.Split('x');
                int largura = Convert.ToInt32(valoresResolucao[0]);
                int altura = Convert.ToInt32(valoresResolucao[1]);
                var service = DependencyService.Get<IPathService>();
                byte[] redimensionado = service.ResizeImageAndroid(Arquivo, largura, altura);
                ArquivoRedimensionado = redimensionado;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        async Task LimparDiretorio()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(TimeSpan.FromSeconds(1));
                String caminho = GetCaminhoPasta();
                if (Directory.Exists(caminho))
                {
                    DirectoryInfo dir = new DirectoryInfo(caminho);
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        fi.Delete();
                    }
                }
                IsLoading = false;
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Sucesso", "Pasta limpa com sucesso");
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<MainPage, String>(new MainPage(), "Erro", ex.Message);
            }            
        }
        async Task<bool> VerificarExistenciaArquivo(String nome)
        {
            try
            {
                String caminho = GetCaminhoPasta();
                if (Directory.Exists(caminho))
                {
                    DirectoryInfo dir = new DirectoryInfo(caminho);
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        if (fi.Name.Equals(nome))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public String GetCaminhoPasta()
        {
            String basePath = DependencyService.Get<IPathService>().Pictures;
            String caminho = Path.Combine(basePath, Pasta);
            return caminho;
        }
    }
}
