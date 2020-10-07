using Petshop.DAL;
using Petshop.Models;
using Petshop.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Petshop.Views
{
    /// <summary>
    /// Lógica interna para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new pageHome();

            //Parece que o erro null acontece porque o context não tem os dados, tipo chamar animal sem ter chamado especie antes
            //então vou chamar todos aqui pra já ficar na memória do context...
            List<Porte> port = PorteDAO.Listar;
            List<Especie> esps = EspecieDAO.Listar;
            List<TipoServico> tpsv = TipoServicoDAO.Listar;
            List<Servico> serv = ServicoDAO.Listar;
            List <AtendimentoServicos> atsv = AtendimentoServicosDAO.Listar;
            List<Cliente> clie = ClienteDAO.Listar;
            List<Funcionario> func = FuncionarioDAO.Listar;
            List<Animal> anim = AnimalDAO.Listar;
        }

        #region Acessibilidade
        private void menuitemAP_Click(object sender, RoutedEventArgs e)
        {
            Utils.Utils.FonteMais(this, this.menu);
        }

        private void menuitemAM_Click(object sender, RoutedEventArgs e)
        {
            Utils.Utils.FonteMenos(this, this.menu);
        }      
        #endregion

        private void menuSair_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void windowMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente sair???", "Petshop Control",
                   MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void menuCliente_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageManterPessoa(true);
        }

        private void menuFuncionario_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageManterPessoa(false);
        }

        private void menuAnimal_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageManterAnimal();
        }

        private void menuServico_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageManterServico();
        }

        private void menuAtendimento_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageManterAtendimento();
        }
        private void menuHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageHome();
        }

        private void menuListarCliente_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageListarDados(0);
        }

        private void menuListarFuncionario_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageListarDados(1);
        }
        private void menuListarAnimal_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageListarDados(2);
        }
        private void menuListarServico_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageListarDados(3);
        }
        private void menuListaAtendimento_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new pageListarDados(4);
        }
    }
}
