using Petshop.DAL;
using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Petshop.Views
{
    /// <summary>
    /// Interaction logic for pageListarPessoas.xaml
    /// </summary>
    public partial class pageListarDados : Page
    {
        private List<dynamic> lstItens = new List<dynamic>();

        public pageListarDados(int intTipo)
        {
            InitializeComponent();
            //impedir usuario de mudar datagrid
            dtaDados.CanUserDeleteRows = false;
            dtaDados.CanUserAddRows = false;
            dtaDados.IsReadOnly = true;

            switch (intTipo)
            {

                default:
                case 0:
                    rdbClientes.IsChecked = true;
                    break;
                case 1:
                    rdbFuncionarios.IsChecked = true;
                    break;
                case 2:
                    rdbAnimais.IsChecked = true;
                    break;
                case 3:
                    rdbServicos.IsChecked = true;
                    break;
                case 4:
                    rdbAtendimentos.IsChecked = true;
                    break;
            }           
        }
        private void MudarPage()
        {
            lstItens.Clear();
            dtaDados.Columns.Clear();

            if ((bool)rdbClientes.IsChecked)
                MudarParaClientes();
            else if ((bool)rdbFuncionarios.IsChecked)
                MudarParaFuncionarios();
            else if ((bool)rdbAnimais.IsChecked) 
                MudarParaAnimais();
            else if ((bool)rdbServicos.IsChecked)
                MudarParaServicos();
            else if ((bool)rdbAtendimentos.IsChecked)
                MudarParaAtendimentos();

            dtaDados.ItemsSource = null;
            dtaDados.ItemsSource = lstItens;
            dtaDados.Items.Refresh();
        }

        private void MudarParaClientes()
        {
            this.lblIdentificarPage.Content = "Lista de Clientes (Dois cliques para ir direto para Alterar)";

            foreach(Cliente cl in ClienteDAO.Listar)
            {
                string strAnimais = "";
                foreach (Animal an in cl.Animais)
                    strAnimais += an.Nome + ", ";

                lstItens.Add(new
                {
                    CPF = cl.Cpf,
                    Nome = $"{cl.Nome} {cl.Sobrenome}",
                    DataNasc = cl.DataNasc,
                    Email = cl.Email,
                    Endereco = cl.Endereco,
                    Telefone = cl.Telefone,
                    Animais = strAnimais
                });
            }
        }

        private void MudarParaFuncionarios()
        {
            this.lblIdentificarPage.Content = "Lista de Funcionários (Dois cliques para ir direto para Alterar)";

            foreach (Funcionario fu in FuncionarioDAO.Listar)
            {               
                lstItens.Add(new
                {
                    CPF = fu.Cpf,
                    Nome = $"{fu.Nome} {fu.Sobrenome}",
                    DataNasc = fu.DataNasc,
                    Matricula = fu.Matricula
                });
            }
        }

        private string PegarDonoDoAnimal(Animal an)
        {
            if (an.Cliente == null)
                return "Sem Dono";

            return $"{an.Cliente.Nome} {an.Cliente.Sobrenome}";
        }

        private void MudarParaAnimais()
        {
            this.lblIdentificarPage.Content = "Lista de Animais (Dois cliques para ir direto para Alterar)";

            foreach (Animal an in AnimalDAO.Listar)
            {
                lstItens.Add(new
                {
                    ID = an.Id,
                    Nome = an.Nome,
                    DataNasc = an.DataNasc,
                    Dono = PegarDonoDoAnimal(an),
                    Esp = an.Especie.Nome,
                    Porte =an.Especie.Porte.Nome
                });
            }
        }

        private void MudarParaServicos()
        {
            this.lblIdentificarPage.Content = "Lista de Serviços (Dois cliques para ir direto para Alterar)";

            foreach (Servico se in ServicoDAO.Listar)
            {
                lstItens.Add(new
                {
                    ID = se.Id,
                    Tipo = se.TipoServico.Nome,
                    Nome = se.Nome,
                    Desc = se.Descricao,
                    Dur = se.Duracao,
                    Preco = se.Preco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR"))
            });
            }
        }
        private void MudarParaAtendimentos()
        {
            this.lblIdentificarPage.Content = "Lista de Atendimentos (Dois cliques para ir direto para Alterar)";

            foreach (Atendimento at in AtendimentoDAO.Listar)
            {
                string strServicos = "";
                foreach (AtendimentoServicos atSv in at.Servicos)
                    strServicos += atSv.Servico.Nome + ", ";

                lstItens.Add(new
                {
                    ID = at.Id,
                    Hora = at.DataHora.ToString("dd/MM/yyyy HH:mm"),
                    Dur = $"{at.Duracao} Minutos",
                    Preco = at.Preco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR")),
                    Func = $"{at.Funcionario.Nome} {at.Funcionario.Sobrenome}",
                    Animal = at.Animal.Nome,
                    Serv = strServicos
                });
            }
        }

        private void dtaDados_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string headername = e.Column.Header.ToString();

            //Cancel the column you don't want to generate
           /* if (headername == "MiddleName")
            {
                e.Cancel = true;
            }*/

            //update column details when generating
            switch (headername)
            {
                case "Serv":
                    e.Column.Header = "Serviços";
                    break;

                case "Func":
                    e.Column.Header = "Funcionário";
                    break;

                case "Desc":
                    e.Column.Header = "Descrição";
                    break;

                case "Tipo":
                    e.Column.Header = "Tipo de Serviço";
                    break;

                case "Dur":
                    e.Column.Header = "Duração";
                    break;

                case "Preco":
                    e.Column.Header = "Preço";
                    break;

                case "DataNasc":
                    e.Column.Header = "Nascimento";
                    break;

                case "Cpf":
                    e.Column.Header = "CPF";
                    break;

                case "Email":
                    e.Column.Header = "E-Mail";
                    break;

                case "Esp":
                    e.Column.Header = "Espécie";
                    break;

                default:
                    break;
            }

        }

        private void rdbChange_Checked(object sender, RoutedEventArgs e)
        {
            MudarPage();
        }

        private void dtaDados_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(((DataGrid)sender).CurrentItem == null)
                    return;

            if ((bool)rdbAnimais.IsChecked)
            {
                int aID = ((dynamic)((DataGrid)sender).CurrentItem).ID;
                NavigationService.Navigate(new pageManterAnimal(aID));
            }
            else if ((bool)rdbServicos.IsChecked)
            {
                int sID = ((dynamic)((DataGrid)sender).CurrentItem).ID;
                NavigationService.Navigate(new pageManterServico(sID));
            }
            else if ((bool)rdbAtendimentos.IsChecked)
            {
                int atID = ((dynamic)((DataGrid)sender).CurrentItem).ID;
                NavigationService.Navigate(new pageManterAtendimento(atID));
            }
            else
            {
                string strCPF = ((dynamic)((DataGrid)sender).CurrentItem).CPF;
                NavigationService.Navigate(new pageManterPessoa((bool)rdbClientes.IsChecked, strCPF));
            }
        }
    }
}
