using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Agenda.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Agenda.Controllers;

[ApiController]
[Route("[controller]")]
public class AgendaController : ControllerBase
{
        static private Pessoa usuario;
        static private int idAgendas;
        static private int idGeral;
        static private int mes;
        static private int ano;

        public AgendaController()
        {
            if( usuario == null )//se não temos um usuario , vamos fazer um
            {
                Incializacao();//incializar como usuario padrao
            }
        }

        bool TelefoneValido(string telefone){
            foreach(char letra in telefone){
                if (!char.IsDigit(letra)){
                    return false;
                }
            }
            return true;//só vai aceitar numeros
        }

        bool AgendaId(Pessoa usuario,int id){
            foreach(_Agenda agenda in usuario.Agendas){
                if(agenda.id==id){
                    return true;
                }
            }
            return false;
        }
        void organizarAgenda(_Agenda agenda){
            agenda.tarefas=agenda.tarefas.OrderBy(c => c.dia).ToList();//vai colocar na lista pela ordem de dias, o c=>c.dia faz uma iteração
            // pelos objetos tarefas e na propriedade dia e usa o dia da tarefa como argumento para a função
            agenda.compromissos=agenda.compromissos.OrderBy(c => c.dia).ToList();      
        }

        void Incializacao()
        {
            usuario=new Pessoa("Usuário");
            idAgendas=0;
            idGeral=0;
            DateTime dataAtual = DateTime.Now;
            mes=dataAtual.Month;
            ano=dataAtual.Year;
        }

        [HttpGet]
        public string BemVindo(){
            return $"Olá ! Você está entrando na sua agenda on-line do mês {mes} e ano {ano}\nAdicione R/User no path para ver suas informações ou /Menu para saber outras opções";
        }
 
        [HttpGet("Menu")]
        public string Menu(){
            return "OBS: os termos precedidos por '_' devem ser trocados pelo nome desejado\n\nAdicione no path:\n\n/R/User para informações do usuário\n/R/Agenda/_id para ver a Agenda do id digitado"+
            "\n/R/Compromisso/_id para ver o compromisso do id digitado\n/R/Tarefa/_id para ver a tarefa do id digitado"+
            "\n\nC/Agenda/_nome-desejado para criar uma nova Agenda\nC/Compromisso/_id-agenda-desejada/_titulo/_hora/_minuto/_dia/duracao para criar um novo compromisso na agenda do id " + 
            "\nC/Tarefa/_id-agenda-desejada/_titulo/_dia para criar uma nova tarefa na agenda do id" +
            "\n\nU/User/_campo-que-deseja-mudar/novo-valor para mudar um dos campos de seu perfil\nU/Agenda/_id/_campo-que-deseja-mudar/novo-valor para mudar um dos campos de sua agenda"+
            "\nU/Compromisso/_id/_campo-que-deseja-mudar/novo-valor para mudar um dos campos de seu compromisso\nU/Tarefa/_id/_campo-que-deseja-mudar/novo-valor para mudar um dos campos de sua tarefa"+
            "\n\nD/Agenda/_id para deletar essa Agenda\nD/Compromisso/_id para deletar esse compromisso\nD/Tarefa/_id para deletar essa tarefa";

        }

///     READ
        [HttpGet("R/User")]
        public ActionResult getUser()
        {
            try{
                foreach(_Agenda agenda in usuario.Agendas){
                    organizarAgenda(agenda);
                }
                return Ok(usuario);
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpGet("R/Agenda/{valor}")]
        public ActionResult getAgenda(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                if(AgendaId(usuario,id)){
                    foreach(_Agenda agenda in usuario.Agendas){
                        if (agenda.id ==id){
                            organizarAgenda(agenda);
                            return Ok(agenda);
                        }
                    }
                }
                return NotFound("Não foi encontrada agenda com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpGet("R/Compromisso/{valor}")]
        public ActionResult getCompromisso(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                foreach(_Agenda agenda in usuario.Agendas){
                    foreach(Compromisso compromisso in agenda.compromissos){
                        if (compromisso.id ==id){
                            return Ok(compromisso);
                        }
                    }
                }
                return NotFound("Não foi encontrado compromisso com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpGet("R/Tarefa/{valor}")]
        public ActionResult getTarefa(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                foreach(_Agenda agenda in usuario.Agendas){
                    foreach(Tarefa tarefa in agenda.tarefas){
                        if (tarefa.id ==id){
                            return Ok(tarefa);
                        }
                    }
                }
                return NotFound("Não foi encontrada tarefa com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

///  DELETE
        [HttpDelete("D/Agenda/{valor}")]
        public ActionResult deleteAgenda(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                if(AgendaId(usuario,id)){
                    foreach(_Agenda agenda in usuario.Agendas){
                        if (agenda.id ==id){
                            usuario.Agendas.Remove(agenda);
                            return Ok("Agenda deletada");
                        }
                    }
                }
                return NotFound("Não foi encontrada Agenda com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpDelete("D/Compromisso/{valor}")]
        public ActionResult deleteCompromisso(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                foreach(_Agenda agenda in usuario.Agendas){
                    foreach(Compromisso compromisso in agenda.compromissos){
                        if (compromisso.id ==id){
                            agenda.compromissos.Remove(compromisso);
                            return Ok("Compromisso deletada");
                        }
                    }
                }
                return NotFound("Não foi encontrado compromisso com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpDelete("D/Tarefa/{valor}")]
        public ActionResult deleteTarefa(string valor)
        {
            try{
                int id;
                id=Convert.ToInt32(valor);
                foreach(_Agenda agenda in usuario.Agendas){
                    foreach(Tarefa tarefa in agenda.tarefas){
                        if (tarefa.id ==id){
                            agenda.tarefas.Remove(tarefa);
                            return Ok("Tarefa deletada");                   
                            }
                    }
                }
                return NotFound("Não foi encontrada tarefa com esse ID");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

///  CREATE 

        [HttpPost("C/Agenda/{valor}")]
        public IActionResult CreateAgenda(string valor){
            try{
                if(string.IsNullOrWhiteSpace(valor)==true){
                    valor="Nova agenda";
                }
                usuario.AddAgenda(idAgendas,valor);
                idAgendas+=1;
                return Ok($"Agenda criada, com id {idAgendas-1}");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpPost("C/Compromisso/{id}/{titulo}/{hora}/{minuto}/{dia}/{duracao}")]
        public IActionResult CreateCompromisso(string id,string titulo,string hora,string minuto,string dia,string duracao){
            try{
                int _id=Convert.ToInt32(id);
                if(!AgendaId(usuario,_id)){
                    return NotFound("Não foi encontrada Agenda com esse ID");
                }
                int _hora=Convert.ToInt32(hora);
                int _minuto=Convert.ToInt32(minuto);
                int _dia=Convert.ToInt32(dia);
                int _mes=mes;
                int _ano=ano;
                double _duracao=Convert.ToDouble(duracao);
                foreach(_Agenda agenda in usuario.Agendas){
                    if(agenda.id==_id){
                        agenda.AddCompromisso(idGeral,titulo,_hora,_minuto,_dia,_mes,_ano,_duracao);
                    }
                }
                idGeral+=1;
                return Ok($"Compromisso criado, com id {idGeral-1}");
            }catch(AgendaException e){
                return BadRequest($"Houve um problema {e.Message}");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }
        [HttpPost("C/Tarefa/{id}/{titulo}/{dia}")]
        public IActionResult CreateTarefa(string id,string titulo,string dia){
            try{
                int _id=Convert.ToInt32(id);
                if(!AgendaId(usuario,_id)){
                    return NotFound("Não foi encontrada Agenda com esse ID");
                }
                int _dia=Convert.ToInt32(dia);
                int _mes=mes;
                int _ano=ano;
                foreach(_Agenda agenda in usuario.Agendas){
                    if(agenda.id==_id){
                        agenda.AddTarefa(idGeral,titulo,_dia,_mes,_ano);
                    }
                }
                idGeral+=1;
                return Ok($"Tarefa criada, com id {idGeral-1}");
            }catch(AgendaException e){
                return BadRequest($"Houve um problema {e.Message}");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

///  UPDATE
        [HttpPut("U/User/{var}/{valor}")]
        public IActionResult UpdateUser(string var,string valor)
        {
            try{
                string campo;
                campo=var.ToUpper();
                if (campo=="NOME" && string.IsNullOrWhiteSpace (valor)==false){
                    usuario.nome=valor;
                }
                else if (campo=="TELEFONE"&& string.IsNullOrWhiteSpace (valor)==false && TelefoneValido(valor)==true){
                    usuario.telefone=valor;
                }
                else if(TelefoneValido(valor)==false){
                    return BadRequest("O telefone só pode conter números");
                }
                else if (campo=="EMAIL"&& string.IsNullOrWhiteSpace (valor)==false){
                    usuario.email=valor;
                }
                else{
                    return BadRequest( $"Campo inválido ou não suportado.{campo}");
                }
                return Ok(usuario);
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpPut("U/Agenda/{id}/{campo}/{novo_valor}")]
        public IActionResult UpdateAgenda(string id,string campo,string novo_valor)
        {
            try{
                int _id=Convert.ToInt32(id);
                if(!AgendaId(usuario,_id)){
                    return NotFound($"Não foi encontrada Agenda com esse ID {_id}");
                }
                string _campo;
                _campo=campo.ToUpper();
                if ((_campo=="TÍTULO" || _campo=="TITULO") && string.IsNullOrWhiteSpace (novo_valor)==false){
                    foreach(_Agenda agenda in usuario.Agendas){
                        if (agenda.id==_id){
                            agenda.titulo=novo_valor;
                            return Ok(agenda);
                        }
                    }
                    return NotFound( $"Id não encontrado");

                }
                else{
                    return BadRequest( $"Campo inválido ou não suportado.{_campo}");
                }
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpPut("U/Compromisso/{id}/{campo}/{novo_valor}")]
        public IActionResult UpdateCompromisso(string id,string campo,string novo_valor)
        {
            try{
                int _id=Convert.ToInt32(id);
                string _campo;
                _campo=campo.ToUpper();
                int i;
                double j;
                if ((_campo=="TÍTULO" || _campo=="TITULO") && string.IsNullOrWhiteSpace (novo_valor)==false){
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Compromisso compromisso in agenda.compromissos){
                            if (compromisso.id==_id){
                                compromisso.titulo=novo_valor;
                                return Ok(compromisso);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else if (_campo=="HORA" && Int32.TryParse(novo_valor,out i)==true){//se tryparse for true, é um numero int e vai pro i
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Compromisso compromisso in agenda.compromissos){
                            if (compromisso.id==_id){
                                compromisso.hora=i;
                                return Ok(compromisso);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else if (_campo=="MINUTO" && Int32.TryParse(novo_valor,out i)==true){//se tryparse for true, é um numero int e vai pro i
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Compromisso compromisso in agenda.compromissos){
                            if (compromisso.id==_id){
                                compromisso.minuto=i;
                                return Ok(compromisso);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else if (_campo=="DIA" && Int32.TryParse(novo_valor,out i)==true){//se tryparse for true, é um numero int e vai pro i
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Compromisso compromisso in agenda.compromissos){
                            if (compromisso.id==_id){
                                compromisso.dia=i;
                                return Ok(compromisso);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else if ((_campo=="DURACAO"||_campo=="DURAÇÃO"||campo=="DURAÇAO"||campo=="DURACÃO") && Double.TryParse(novo_valor,out j)==true){//se tryparse for true, é um numero int e vai pro i
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Compromisso compromisso in agenda.compromissos){
                            if (compromisso.id==_id){
                                compromisso.duracao=j;
                                return Ok(compromisso);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else{
                    return BadRequest( $"Campo inválido ou não suportado.{_campo}");
                }
            }catch(AgendaException e){
                return BadRequest($"Houve um problema {e.Message}");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        [HttpPut("U/Tarefa/{id}/{campo}/{novo_valor}")]
        public IActionResult UpdateTarefa(string id,string campo,string novo_valor)
        {
            try{
                int _id=Convert.ToInt32(id);
                string _campo;
                _campo=campo.ToUpper();
                int i;
                if ((_campo=="TÍTULO" || _campo=="TITULO") && string.IsNullOrWhiteSpace (novo_valor)==false){
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Tarefa tarefa in agenda.tarefas){
                            if (tarefa.id==_id){
                                tarefa.titulo=novo_valor;
                                return Ok(tarefa);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else if (_campo=="DIA" && Int32.TryParse(novo_valor,out i)==true){//se tryparse for true, é um numero int e vai pro i
                    foreach(_Agenda agenda in usuario.Agendas){
                        foreach(Tarefa tarefa in agenda.tarefas){
                            if (tarefa.id==_id){
                                tarefa.dia=i;
                                return Ok(tarefa);
                            }
                        }
                    }
                    return NotFound( $"Id não encontrado");
                }
                else{
                    return BadRequest( $"Campo inválido ou não suportado.{_campo}");
                }
            }catch(AgendaException e){
                return BadRequest($"Houve um problema {e.Message}");
            }catch(Exception e){
                return BadRequest($"Houve um problema {e.Message}");
            }
        }

        

}

