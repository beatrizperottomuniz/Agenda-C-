using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Agenda.Models
{
    public class _Agenda
    {
        public int id{get;}
        private string _titulo;
        public string titulo {get
        {
            return this._titulo;
        }
        set{
            if (string.IsNullOrWhiteSpace(value)==true){
                throw new AgendaException("A agenda precisa ter um título");
            }else{
                this._titulo=value;
            }
        }}
        public List<Tarefa> tarefas{get;set;}
        public List<Compromisso> compromissos{get;set;}


        public _Agenda()
        {
            
        }

        public _Agenda(int id,string titulo){
            this.titulo=titulo;
            this.id=id;
            tarefas= new List<Tarefa>();
            compromissos=new List<Compromisso> ();
        }

        public void AddCompromisso(int id,string titulo,int hora,int minuto,int dia,int mes,int ano,double duracao){
            compromissos.Add(new Compromisso( id, titulo, hora, minuto, dia, mes, ano, duracao));
        }
        public void AddTarefa(int id,string titulo,int dia,int mes,int ano){
            tarefas.Add(new Tarefa( id, titulo, dia, mes, ano));
        }

    }
}