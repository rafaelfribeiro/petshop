using Petshop.DAL;
using Petshop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
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
    /// Interaction logic for pageManterAtendimento.xaml
    /// </summary>
    public partial class pageManterAtendimento : Page
    {
        private Atendimento atendimento;

        private List<dynamic> lstOcupados; //para dtaOcupado

        private List<Servico> lstServicos = new List<Servico>();

        private List<dynamic> lstItens = new List<dynamic>();

        private int intDuracaoMinutos = 0;

        private double dblPreco = 0;

        public pageManterAtendimento(int id = -1)
        {
            InitializeComponent();

            dtaServicos.CanUserAddRows = false;
            dtaOcupado.CanUserAddRows = dtaOcupado.CanUserDeleteRows = false;

            dtaAtendimentos.CanUserAddRows = dtaAtendimentos.CanUserDeleteRows = false;
            dtaAtendimentos.IsReadOnly = true;

            LimparFormulario();

            cboDono.ItemsSource = ClienteDAO.Listar;
            cboDono.DisplayMemberPath = "Nome";
            cboDono.SelectedValuePath = "Id";

            cboFuncionario.ItemsSource = FuncionarioDAO.Listar;
            cboFuncionario.DisplayMemberPath = "Nome";
            cboFuncionario.SelectedValuePath = "Id";

            cboServico.ItemsSource = ServicoDAO.Listar;
            cboServico.DisplayMemberPath = "Nome";
            cboServico.SelectedValuePath = "Id";

            if(id != -1)
            {
                txtID.Text = id.ToString();
                Buscar();
            }
        }

        //Achar um jeito de chamar quando sercviço for deletado...
        private void UpdateDuracaoPrecoAte()
        {
            intDuracaoMinutos = 0;
            dblPreco = 0;

            foreach (Servico serv in lstServicos)
            {
                intDuracaoMinutos += serv.Duracao;
                dblPreco += serv.Preco;
            }

            lblDurPre.Content = $"{intDuracaoMinutos} Minutos/{dblPreco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR"))}";

            if (intDuracaoMinutos == 0 || String.IsNullOrWhiteSpace(txtHora.Text) || txtHora.Text.Length != 5 || !(txtHora.Text.EndsWith(":00") || txtHora.Text.EndsWith(":15") || txtHora.Text.EndsWith(":30") || txtHora.Text.EndsWith(":45")))
                return;
            try
            {
                Convert.ToInt32(txtHora.Text.Split(':')[0]);
            }
            catch
            {
                return;
            }

            DateTime dtTemp = Convert.ToDateTime($"01/01/2000 {txtHora.Text}:00");
            dtTemp = dtTemp.AddMinutes(intDuracaoMinutos);
            lblHoraAte.Content = dtTemp.ToString("HH:mm");
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cboServico.SelectedIndex == -1 || lstServicos.Contains((Servico)cboServico.SelectedItem))
            {
                MessageBox.Show("Selecione um serviço ou Serviço selecionado já está na lista.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            lstServicos.Add((Servico)cboServico.SelectedItem);

            dtaServicos.ItemsSource = lstServicos;
            dtaServicos.Items.Refresh();

            UpdateDuracaoPrecoAte();
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            UpdateDuracaoPrecoAte();

            if (CamposVazios() || !ValidarHorarioDia() || !ChecarConflitoDeHorario())
                return;           

            atendimento = new Atendimento
            {
                Animal = (Animal)cboAnimal.SelectedItem,
                Funcionario = (Funcionario)cboFuncionario.SelectedItem,
                Duracao = intDuracaoMinutos,
                Preco = dblPreco
            };

            string strTemp = dtpDia.SelectedDate.ToString().Substring(0, dtpDia.SelectedDate.ToString().IndexOf(" "));
            strTemp = strTemp + $" {txtHora.Text}:00";

            atendimento.DataHora = Convert.ToDateTime(strTemp);

            /*if (AtendimentoDAO.BuscarPorAnimalIdEDia(atendimento.Animal.Id, atendimento.DataHora).Count > 0)
            {
                MessageBox.Show("Animal já possui Atendimento marcado no Dia selecionado", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }*/

            //não sei se pega, tem que testar...
            foreach (Servico sv in lstServicos)
                atendimento.Servicos.Add(new AtendimentoServicos { Atendimento = atendimento, Servico = sv });

            if (AtendimentoDAO.Cadastrar(atendimento))
            {
                MessageBox.Show("Atendimento cadastrado com sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparFormulario(); 
                Buscar();
            }
            else
                MessageBox.Show("Algo deu errado na hora de cadastrar.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Buscar()
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                lstItens.Clear();
                //dtaServicos.Items.Clear();// limpe apenas a lista, não precisa limpar o datagrid, nem pode, da erro
                foreach (Atendimento at in AtendimentoDAO.Listar)
                {
                    string strServicos = "";
                    foreach (AtendimentoServicos atSv in at.Servicos)
                        strServicos += atSv.Servico.Nome + ", ";

                    lstItens.Add(new
                    {
                        ID = at.Id.ToString(),
                        Hora = at.DataHora.ToString("HH:mm"),
                        Dura = at.Duracao,
                        preco = at.Preco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR")),
                        Func = $"{at.Funcionario.Nome} {at.Funcionario.Sobrenome}",
                        Animal = at.Animal.Nome,
                        Serv = strServicos
                    });
                }
                dtaAtendimentos.ItemsSource = lstItens;
                dtaAtendimentos.Items.Refresh();
                //não gosto de ficar colocando um monte de else, se for vários vai encadeando e jogando tudo pro lado...
                return;
            }

            atendimento = AtendimentoDAO.BuscarPorID(Convert.ToInt32(txtID.Text));

            if (atendimento == null)
            {
                MessageBox.Show("Atendiemnto não encontrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //O animal tem que ter dono pra poder ser cadastrato, mas se ele for modificado depois...
            if (atendimento.Animal.Cliente != null)
                cboDono.SelectedIndex = cboDono.Items.IndexOf(atendimento.Animal.Cliente);

            cboAnimal.SelectedIndex = cboAnimal.Items.IndexOf(atendimento.Animal);
            cboFuncionario.SelectedIndex = cboFuncionario.Items.IndexOf(atendimento.Funcionario);

            dtpDia.SelectedDate = atendimento.DataHora;

            txtHora.Text = atendimento.DataHora.ToString("HH:mm");

            foreach (AtendimentoServicos atSv in atendimento.Servicos)
                lstServicos.Add(atSv.Servico);

            dtaServicos.ItemsSource = lstServicos;
            dtaServicos.Items.Refresh();

            lblDurPre.Content = $"{atendimento.Duracao} Minutos/{atendimento.Preco.ToString("C2", CultureInfo.GetCultureInfoByIetfLanguageTag("pt-BR"))}";

            btnRemover.IsEnabled = true;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = btnAdd.IsEnabled = txtID.IsEnabled = dtaServicos.CanUserDeleteRows = false;
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            Buscar();
        }

        private void btnRemover_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                MessageBox.Show("ID vazio.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AtendimentoDAO.Remover(atendimento);
            MessageBox.Show("Atendimento Removido com sucesso.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Information);
            LimparFormulario();
            Buscar();
        }

        private void btnLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparFormulario();
        }

        private void cboDono_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboDono.SelectedIndex == -1)
                return;

            cboAnimal.ItemsSource = AnimalDAO.BuscarPorClienteId((int)cboDono.SelectedValue);
            cboAnimal.DisplayMemberPath = "Nome";
            cboAnimal.SelectedValuePath = "Id";

            if(cboAnimal.Items.Count == 0)
                MessageBox.Show("Cliente não possui Animal Cadastrado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);

        }
        
        private void cboFuncionarioOudtpDia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboFuncionario.SelectedIndex == -1 || dtpDia.SelectedDate == null || cboAnimal.SelectedIndex == -1)
                return;

            //Procurar Atendimentos no dia do funcionário
            List<Atendimento> lstAtendimentosFuncionario = AtendimentoDAO.BuscarPorFuncionarioIdEDia((int)cboFuncionario.SelectedValue, (DateTime)dtpDia.SelectedDate);           
            List<Atendimento> lstAtendimentosAnimal = AtendimentoDAO.BuscarPorAnimalIdEDia((int)cboAnimal.SelectedValue, (DateTime)dtpDia.SelectedDate);

            List<Atendimento> lstAtendimentos = new List<Atendimento>();

            //Concact
            foreach (Atendimento at in lstAtendimentosFuncionario)
                if (!lstAtendimentos.Contains(at))
                    lstAtendimentos.Add(at);

            foreach (Atendimento at in lstAtendimentosAnimal)
                if (!lstAtendimentos.Contains(at))
                    lstAtendimentos.Add(at);

            lstOcupados = new List<dynamic>();

            if (lstAtendimentosFuncionario.Count == 0 && lstAtendimentosAnimal.Count == 0)
            {
                dtaOcupado.Columns.Clear();
                dtaOcupado.ItemsSource = null;
                return;
            }

            foreach (Atendimento at in lstAtendimentos)
            {
                DateTime dtFim = at.DataHora.AddMinutes(at.Duracao);
                lstOcupados.Add(new
                {
                    Inicio = $"{at.DataHora.Hour}:{at.DataHora.Minute}",
                    Fim = $"{dtFim.Hour}:{dtFim.Minute}"
                });
            }

            //Jogar horarios ocupados no datagrid
            dtaOcupado.ItemsSource = lstOcupados;
            dtaOcupado.Items.Refresh();
        }

        private void LimparFormulario()
        {
            txtID.Clear();
            txtHora.Clear();
            lstServicos.Clear();

            lblHoraAte.Content = "";

            cboDono.SelectedIndex = -1;
            cboAnimal.SelectedIndex = -1;
            cboFuncionario.SelectedIndex = -1;
            cboServico.SelectedIndex = -1;

            atendimento = new Atendimento();

            lstOcupados = null;
            dtpDia.SelectedDate = null;
            dtaOcupado.ItemsSource = null;
            dtaServicos.ItemsSource = null;

            cboDono.Focus();

            btnRemover.IsEnabled = false;
            btnCadastrar.IsEnabled = btnBuscar.IsEnabled = btnAdd.IsEnabled = txtID.IsEnabled = dtaServicos.CanUserDeleteRows = true;
        }

        private bool ValidarHorarioDia()
        {
            if(dtpDia.SelectedDate < DateTime.Now.AddDays(-1))
            {
                MessageBox.Show("Dia Inválido, selecione um dia não passado.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                int intHora = Convert.ToInt32(txtHora.Text.Split(':')[0]);
                if (intHora < 6 || intHora > 20)
                {
                    MessageBox.Show("Horário Inválido, entre com Horário Entre 06:00 e 20:00.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch
            {
                return false;
            }

            if (txtHora.Text.Length != 5 || !(txtHora.Text.EndsWith(":00") || txtHora.Text.EndsWith(":15") || txtHora.Text.EndsWith(":30") || txtHora.Text.EndsWith(":45")))//13:00
            {
                MessageBox.Show("Horário Inválido, entre com Horário no formato HH:MM terminando em 00, 15, 30 ou 45.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool ChecarConflitoDeHorario()
        {

            DateTime dtAtInicio = Convert.ToDateTime($"01/01/2000 {txtHora.Text}:00");
            DateTime dtAtFim = dtAtInicio.AddMinutes(intDuracaoMinutos);

            foreach (dynamic oc in lstOcupados)
            {
                DateTime dtOcInicio = Convert.ToDateTime($"01/01/2000 {oc.Inicio}:00");
                DateTime dtOcFim = Convert.ToDateTime($"01/01/2000 {oc.Fim}:00");

                if ((dtAtInicio >= dtOcInicio && dtAtInicio < dtOcFim) || (dtAtFim > dtOcInicio && dtAtFim <= dtOcFim))
                {
                    MessageBox.Show("Horário em Conflito, entre com novo Horário.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            return true;
        }
		
        private bool CamposVazios()
        {
            if (cboDono.SelectedIndex == -1 
                || cboAnimal.SelectedIndex == -1 
                || cboFuncionario.SelectedIndex == -1 
                || !dtpDia.SelectedDate.HasValue 
                || String.IsNullOrWhiteSpace(txtHora.Text) 
                || lstServicos.Count == 0)
            {
                MessageBox.Show("Um ou Mais Campos Vazios.", "Pet Shop", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void dtaAtendimentos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            txtID.Text = (string)((dynamic)((DataGrid)sender).CurrentItem).ID;
            Buscar();
        }
         
        private void txtHora_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateDuracaoPrecoAte();
        }
    }
}
