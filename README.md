# LP1_ZombiesVsHumans

## Zombie vs Humans

* André Vitorino nº 21802937
  
* Carolina Bastos nº 21802937

**Repositório do GitHub:** [GitHub](https://github.com/Freeze88/LP1_ZombiesVsHumans)

O André começou a fazer o projeto e criar as classes: '*Application*', '*MAP*', '*Math*', '*Player*' , '*Zombie*', a classe '*Program*' foi alterada. De seguida, fez alterações em algumas classes, adicionado inteligência artificial e uma maneira de controlar o jogador.Criou mais 4 classes. Acabou por corrigir alguns problemas em certas classes e comentou o código. Fez também o diagrama em UML e o fluxograma.

A Carolina começou por criar a classe EmptySpace. De seguida, criou mais classes e desenvolveu. Foi fazendo alterações de modo a tornar o código mais eficiente e com menos código morto.
Fez o relatório.

### Descrição da Solução

* O mapa é criado "internamente" sendo um array de tudo o que está o mapa incluindo espaços vazios, este guarda as posições, valores (Hashes) e o tipo de objeto lá (humano ou zombie), ele é criado vizualmente nas suas partes respetivas, por exemplo a class human apresenta os humanos, os zombies apresenta os mesmos e a classe espaço vazio apresenta-se visualmente dentro de si própria.

* A IA funciona por base de "Hashes", de forma simples os Zombies e humanos formam hashes á sua volta tendo o menor valor onde estão e o maior valor no lugar em que a distãncia é maior, De seguida os zombies procuram nas Hashes dos humanos qual é o menor valor na área circundante e movem-se nessa direção. Os humanos fazem o mesmo exceto que vão para o maior número.

* O Fluxograma e o diagrama UML encontram-se na pasta do projeto.

* Para jogar o jogo basta abrir o cmd na pasta que contem as classes do programa e meter as seguintes informações nesta ordem: x(largura do mapa), y(altura do mapa), número de zombies(z), número de humanos(h), número de humanos controláveis(H) e número de turnos(t). Exemplo: ```dotnet run -- -x 10 -y 10 -z 1 -h 1 -H 1 -t 100```

### Bugs

* A IA faz default a ir para a direita;
* As *Hashes* nem sempre são bem calculadas;
* Os humanos e os Zombies podem ficar, infinitamente, a andar numa direção.

### Conclusão

Concluindo, tivemos ajuda de um colega e recorremos tanto a [.NET API](https://docs.microsoft.com/pt-pt/dotnet/api/), como ao [StackOverflow](https://stackoverflow.com/).

No caso do colega, ele ajudou-nos na utilização de *partial classes* e de *event handlers* para além da ajudada com a lógica do programa. No que toca as outras fontes, a API foi a mais utilizada ao longo do trabalho para diversas ocasiões, como por [exemplo](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run?tabs=netcore21), no StackOverflow, [isto](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run?tabs=netcore21). Por fim, recorremos aos *slides* lecionados em aula.

Ao longo do projeto fomos aprendendo e consolidando a matéria dada nas aulas. Uma das principais matérias foi as heranças, esta, foi essencial para o programa.

### Referências

[StackOverflow](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run?tabs=netcore21)

[.NET API](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-run?tabs=netcore21)