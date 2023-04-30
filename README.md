# Simulação de ecossistemas
## Feito por:
- Rodrigo Pires 22103008
- João Inácio 22202654

## Divisão de trabalho

- IA Design: Rodrigo
- Relatório: Rodrigo
- Base das state machines: Rodrigo
- Implementação do movimento dos peixes: João
- Implementação da deteção dos peixes e algas: João
- Implementação da ação de comer e crescer de algas:  João

## Introdução

O problema presente neste projecto é como representar, de maneina mininamente objetiva, o comportamento de um ecosistema aquático. A resolução encontrada foi o uso de _State machines_ para simular os peixes presentes na simulação assim como as próprias algas que neste projeto foram tratadas da mesma maneira.
Depois de alguma investigação o resultado deve ser uma oscilação constante entre as 4 espécies presentes na simulação, de acordo com a abundância da suas fontes de alimento.

## Metodologia 

A simulação foi feita em 3D usando agentes de movimento dinâmico.
Todos os valores parametrizáveis:
- a energia que cada fonte de alimento dá quando é consumida
- a velocidade e aceleração máxima de cada peixe, linear e angular
- o que cada peixe considera alimento
- o alcance de deteção de cada peixe
- o alcance de comer
- a taxa a qual peixes perdem energia
- a energia máxima que um peixe pode ter
- a frequência de momentos onde uma alga pode crescer e a probablidade de isso acontecer
- o limiar de energia para reprodução

Os gráficos representates de todas as State Machines encontram-se no projecto.

## Discusão

A simulação teve o resultado esperado mas com algumas reações interessantes quando se mexia demasiado com os parametros dentro dela dês de:
- eventos de extinção quando se aumenta demasiado a taxa de perda de energia
- números reduzidos de peixes quando o limiar de energia para a reprodução é elevado
- números de peixes reduzidos quando algas crescer mais rapidamente
- um comportamento mais competitivo de cada peixe quando a energia de cada fonte de alimento é reduzida


## Conclusões

Em conclusão a simulação revelou que o balanceamento de ecossistema aquático é bastante delicado e necessita de certos parametros para poder existir em harmonia. Apesar de termos usado uma metodologia minimamente simples, conseguimos replicar comportamentos complexos de sistemas mais aperfundados.

## Referências
O professor Nuno Fachada
Millington, I. (2019). AI for Games (3rd ed.). CRC Press.
Huisman J.,  Weissing F.J.. Biodiversity of plankton by species oscillations and chaos, Nature, 1999, vol. 402 (pg. 407-410)
[Large-scale variation in density of an aquatic ecosystem indicator species](https://www.nature.com/articles/s41598-018-26847-x)



