from random import randint

def Normalize(i, against = 56, base = 0):
    while(True):
        if(i < 0):
            t = (against + base) + i
            if( t > 0 ) : return t
            i = t;
            continue

        if (i >= (against + base)):
            t = i - against
            if(t < (against + base)) : return t
            i = t
            continue
        if(i < (against + base)) : return i

        break

def RollDice():
    return randint(1,6)


class Board(object):
    PawnsPerPlayer = 4
    PlayerNumber = 4

    

    def __init__(self):
        self.Players = []
        self._lastPlayer = 0
        self.PlayerId = -1

    def NextPlayer(self):
        return Normalize(self._lastPlayer, PlayerNumber)


    def generate_players(self):
        self.Players.clear()
        for x in range(1, PlayerNumber):
            new_p = Player()
            new_p.NextMove = None
            self.Players.append(new_p)






