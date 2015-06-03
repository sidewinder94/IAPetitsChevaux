Classic = CaseType("c-{0}")
EndGame = CaseType("e-{0}-{1}")
Square = CaseType("sq-{0}")


class CaseType(object):
    
    def __init__(self, value):
        self.Value = value

    def __eq__(self, other):
        return self.Value == other.Value

    def __ne__(self, other):
        return not self.__eq__(other)

    def __str__(self):
        return self.Value

    def __repr__(self):
        return self.__str__(self)

