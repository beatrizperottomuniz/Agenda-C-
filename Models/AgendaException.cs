class AgendaException : ApplicationException{
    public string sugestaoCorrecao {get;set;}

    public AgendaException(string sugestao){
        this.sugestaoCorrecao = sugestao;
    }
}
//essa classe serve para, se usarem as classes fora das funções feitas, podermos garantir que os campos não serão preenchidos indevidamente.