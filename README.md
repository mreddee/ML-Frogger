# 🐸 ML-Frogger

Repository github per il progetto del corso Computer Graphics e Multimedia 2021-2022.

## Membri del gruppo

- Cuicchi Manila
- Giannelli Edoardo
- Pierigè Giacomo

## Obiettivo del gioco

L'obiettivo del gioco è quello di occupare tutte e 5 le postazioni (homes) poste dall'altra parte della riva senza essere colpiti dai veicoli ed evitando di cadere in acqua. L'agente ha a disposizione 30 secondi per superare il livello. Superato il livello, il gioco ricomincia dall'inizio.

## Esecuzione del gioco

Premendo il pulsante play il gioco viene avviato nella modalità di inferenza. Il gameplay viene controllato da un agente addestrato a superare i vari livelli del gioco. Per controllare il gameplay tramite l'input dell'utente è necessario impostare l'opzione *heuristic only*.

Per un corretto funzionamento del gioco è necessario impostare un rapporto d'aspetto 1:1 per l'inquadratura.

## Strategia utilizzata

Il modello utilizzato dall'agente per prevedere le azioni da compiere è stato addestrato utilizzando il toolkit di Unity ML-Agents. Il modello utilizzato è stato ricavato dopo 5 milioni di steps di training. Per l'addestramento si è deciso di usare la strategia dell'imitation learning. Questa prevede di utilizzare delle dimostrazioni del gioco registrate dall'utente per migliorare le performance dell'agente.

## Modifiche apportate al gioco

- Eliminato il game over
- Eliminate le vite e il relativo contatore (quando l'agente perde una vita il livello ricomincia da capo)
- Aggiunto un contatore per i livelli superati
- Aggiunto un contatore per le morti
- Aggiunto un contatore per le case occupate
- Sistemati alcuni bug del gioco

## Ricompense

- +3 per ogni livello superato
- +2 per ogni casa occupata
- +0.05 per ogni passo in avanti fatto sulla strada
- +0.5 per ogni passo in avanti fatto sui tronchi o sulle tartarughe

## Link al gioco originale

https://github.com/Blankeos/frogger-unity

## Link ai file di configurazione di esempio proposti da ML-Agents

https://github.com/Unity-Technologies/ml-agents/tree/main/config/imitation

## Demo

Al seguente link è disponibile una demo registrata dell'agente mentre supera i primi 5 livelli del gioco: https://we.tl/t-urzF9FHtge

<p align="center"><img width="500" height="500" src="https://github.com/Giacomo-pierig/ML-Frogger/blob/main/demo.gif"></p>
