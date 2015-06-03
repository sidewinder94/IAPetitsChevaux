import CaseType
import Board

class Pawn(object):
    pass

    def __init__(self):
        self.OldType = CaseType.Square
        self.OldPosition = 0

    def __init__(self, caseType, position):
        self.__init__()
        self.Type = caseType
        self.Position = position

    def __str__(self):
        return self.Type.__str__() + self.Position

    def __repr__(self):
        return self.__str__()

    def __eq__(self, other):
        return self.Position == other.Position and self.Type == other.Type

    def __ne__(self, other):
        return not self.__eq__(other)

    def NoMove(self):
        self.OldPosition = self.Position
        self.OldType = self.Type

    def MoveTo(self, type, position, board):
        if(board is None) : raise ValueError("Board should not be null")

        if (type == CaseType.Square):
            self.OldPosition = self.Position
            self.OldType = self.Type
            self.Type = type
            self.Position = position

        owner = next(f for f in board if self in f.Pawns)
        newPosition = Board.Normalize(position)
        newType = type

        if(type == CaseType.Classic):
            for player in board:
                count = sum(1 for f in player.Pawns if f.Position == newPosition and f.Type == newType)

                if(count == 1 and player != owner):
                    removed = next(f for f in player.Pawns if f.Position == newPosition and f.Type == newType)
                    removed.MoveTo(CaseType.Square, 0)

                if(count == 2):
                    newType = self.Type
                    newposition = self.Position

        self.OldPosition = self.Position
        self.OldType = self.Type
        
        self.Position = newPosition
        self.Type = newType

        return self

    def Move(self, roll, board):
        if (board is None) : raise ValueError("Board should not be null")

        owner = next(f for f in board if self in f.Pawns)
        newPosition = Board.Normalize(position)

        self.MoveTo(CaseType.Classic, Board.Normalize(self.Position + roll), board)

        return self

    def Clone(self):
        n = Pawn()
        n.OldPosition = self.OldPosition
        n.OldType = self.OldType
        n.Type = self.Type
        n.Position = self.Position


    @property
    def TypeConverter(self):
        return self.Type.__str__

    @TypeConverter.setter
    def TypeConverter(self, value):
        self.Type = self._typeConverter(value)

    @property
    def Position(self):
        return self.Position

    @Position.setter
    def Position(self, x : int):
        Board.Normalize(x)

    def _typeConverter(self, typeString):
        if(typeString == CaseType.Classic.Value):
            return CaseType.Classic
        if(typeString == CaseType.Square.Value):
            return CaseType.Square
        if(typeString == CaseType.EndGame.Value):
            return CaseType.EndGame
        


