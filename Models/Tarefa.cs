using System;

namespace Agenda.Models
{
    public class Tarefa
    {
        public int id{get;}
        private DateTime atual;
        private int diaMaximo;
        private string _titulo;
        private int _dia;
        private int _mes;
        private int _ano;
        public string titulo {
            get{
                return this._titulo;
            }
            set{
                if (string.IsNullOrWhiteSpace(value)==true){
                        throw new AgendaException("O compromisso precisa de título");
                }else{
                    this._titulo=value;
                }
            }}
        public int dia {
            get{
                return this._dia;
            }
            set{

                if (value<atual.Day || value>this.diaMaximo){
                        throw new AgendaException("Esse dia não é válido");
                }else{
                    this._dia=value;
                }
            }}
        public int mes {
            get{
                return this._mes;
            }
            set{

                if (value!=atual.Month){
                        throw new AgendaException("Esse mês não é válido");
                }else{
                    this._mes=value;
                }
            }}
        public int ano {
            get{
                return this._ano;
            }
            set{

                if (value!=atual.Year){
                        throw new AgendaException("Esse ano não é válido");
                }else{
                    this._ano=value;
                }
            }}

        public Tarefa()
        {
            
        }

        public Tarefa(int id,string titulo,int dia,int mes,int ano){
            this.atual= DateTime.Now;
            this.diaMaximo = DateTime.DaysInMonth(atual.Year, atual.Month);
            this.id=id;
            this.titulo=titulo;
            this.dia=dia;
            this.mes=mes;
            this.ano=ano;
        }

    }
}