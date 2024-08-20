# Mine Sweeper

## Objetivo
O objetivo do jogo consiste em revelar todos os campos sem provocar uma explosão.

### Campo
- Representa uma entidade do jogo com a qual o jogador pode interagir.
- Possui dois estados:
    1. Escondido
    2. Revelado
- O jogador só pode interagir com campos que estão no estado Escondido (1)
- O estado inicial de todos os campos é escondido, até que haja uma iteração
- Um campo pode conter dentro de si uma bomba
- O jogador pode escolher iteragir com um campos das seguintes formas:
    1. Revelar: Revela um campo escondido, podendo gerar:
        1. Explosão: Caso o campo contenha uma bomba, é gerada uma explosão.
        2. Contagem: Caso o campo não contenha uma bomba, ele passa a disponibilizar a quantidade de bomba que existem nos campos adjacentes.
        Obs: Caso não existe nenhuma bomba nas adjacencias, é gerada uma reação em cadeia de revelação dos campos adjacentes.
    2. Marcando: marca um campo com um dos valores abaixo
        1. Com bomba: Indica que o campo está escondendo uma bomba
        2. Sem bomba: Indica que não existe uma bomba escondida no campo
- Todo campo possui uma identificação, que é um ponto no primeiro quadrante de um plano cartesiano (X/Y)
- Todo campo deve conhecer os campos adjacentes a si
- Todo campo deve expor aos seus adjacentes se está escondendo uma bomba

### Tabuleiro
- Representa o primeiro quadrante de um plano cartesiano e é onde estão posicionados todos os campos
- Para cada ponto do tabuleiro só pode existir um campo
- Todo tabuleiro possui um número de bombas pré-definido que serão distribuidas aleatóriamente entre os campos posicionados nele
- Todo tabuleiro tem sua quantidade de colunas (x) e linhas (y) definidas antes de sua criação
- A quantidade de campos posicionados em um tabuleiro é definido pela quantidade de linhas (y) multiplicada pela quantidade de colunas (x)
- A quantidade de colunas deve ser maior ou igual a 1
- A quantidade de linhas deve ser maior ou igual a 1
- A quantidade de bombas não pode ser maior que a quantidade total de campos