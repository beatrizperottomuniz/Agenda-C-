using System;

namespace Agenda.Models
{
    public class Pessoa
    {
        private string _nome;
        private string _telefone;
        private string _email;
        public string nome {
            get{
                return this._nome;
            }
            set{
                if (string.IsNullOrWhiteSpace(value)==true){
                        throw new AgendaException("O usuário precisa de um nome");
                }else{
                    this._nome=value;
                }
            }}
        public string telefone {
            get{
                return this._telefone;
            }
            set{
                if (string.IsNullOrWhiteSpace(value)==true){
                        throw new AgendaException("O telefone não pode ser nulo");
                }
                foreach(char letra in telefone){
                    if (!char.IsDigit(letra)){
                        throw new AgendaException("O telefone não pode ser nulo");;
                    }
                }
                this._telefone=value;
            }}
        public string email {
            get{
                return this._email;
            }
            set{
                if (string.IsNullOrWhiteSpace(value)==true){
                        throw new AgendaException("O email não pode ser nulo");
                }else{
                    this._email=value;
                }
            }}
        public List<_Agenda> Agendas{get;set;}


        public Pessoa()
        {
            
        }
        public Pessoa(string nome){
            this.nome=nome;
            Agendas= new List<_Agenda>();
        }
        public void AddAgenda(int id,string titulo){
            Agendas.Add(new _Agenda(id,titulo));
        }

    }
}