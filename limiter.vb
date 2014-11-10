Public Class limiter
    Public cellsPosAndBorder As New HashSet(Of Point)
    Public cellsPos As New HashSet(Of Point)

    Public Sub addCellPosAndBorder(ByVal x, ByVal y)
        CellsPos.Add(New Point(x, y))
        cellsPosAndBorder.Add(New Point(x, y))
        For Each i In {-1, 1}
            For Each j In {-1, 1}
                cellsPosAndBorder.Add(New Point(x + i, y + j))
            Next
        Next

        For Each i In {0}
            For Each j In {-1, 1}
                CellsPosAndBorder.Add(New Point(x + i, y + j))
            Next
        Next

        For Each i In {-1, 1}
            For Each j In {0}
                CellsPosAndBorder.Add(New Point(x + i, y + j))
            Next
        Next
    End Sub

    Public Sub updateCellsPosAndBorder()
        cellsPosAndBorder.Clear()
        For Each pos In cellsPos
            addCellPosAndBorder(pos.X, pos.Y)
        Next
    End Sub
    Public Sub clearCellsPos()
        cellsPos.Clear()
    End Sub

    Public Sub clearBothSets()
        cellsPos.Clear()
        cellsPosAndBorder.Clear()
    End Sub

    Public Sub addCellsPos(ByVal x, ByVal y)
        cellsPos.Add(New Point(x, y))
    End Sub
End Class
