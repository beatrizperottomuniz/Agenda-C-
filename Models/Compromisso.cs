using System;

namespace Agenda.Models
{
    public class Compromisso
    {
        public int id{get;}
        private DateTime atual;
        private int diaMaximo;
        private string _titulo;
        private int _hora;
        private int _minuto;
        private int _dia;
        private int _mes;
        private int _ano;
        private double _duracao; 
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
        public int hora {
            get{
                return this._hora;
            }
            set{
                if (value<0 || value>23){
                        throw new AgendaException("Essa hora não é válida");
                }else{
                    this._hora=value;
                }
            }}
        public int minuto {
            get{
                return this._minuto;
            }
            set{
                if (value<0 || value>59){
                        throw new AgendaException("Esse minuto não é válido");
                }else{
                    this._minuto=value;
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
        public double duracao {
            get{
                return this._duracao;
            }
            set{

                if (value<0){
                        throw new AgendaException("Essa duração não é válida");
                }else{
                    this._duracao=value;
                }
            }}



        public Compromisso()
        {
            
        }

        public Compromisso(int id,string titulo,int hora,int minuto,int dia,int mes,int ano,double duracao){
            this.atual= DateTime.Now;
            this.diaMaximo = DateTime.DaysInMonth(atual.Year, atual.Month);
            this.id=id;
            this.titulo=titulo;
            this.hora=hora;
            this.minuto=minuto;
            this.mes=mes;
            this.dia=dia;
            this.ano=ano;
            this.duracao=duracao;
        }

    }
}