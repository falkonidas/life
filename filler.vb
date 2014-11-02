Public Class filler
    Private two_cell_pos As New List(Of Point)
    Private Event twoPointsReached()
    Private midpoint As Point
    Public Sub get_pos(ByVal cell_pos, ByVal pic_pos)
        two_cell_pos.Add(cell_pos)
        If two_cell_pos.Count = 2 Then
            RaiseEvent twoPointsReached()
        End If
    End Sub
    Private Sub get_line() Handles Me.twoPointsReached

        Dim startCellPos = two_cell_pos(0)
        Dim endCellPos = two_cell_pos(1)

        If startCellPos <> endCellPos Then
            Dim bre As New BresenhamsLineAlgorithm(startCellPos.X, startCellPos.Y, endCellPos.X, endCellPos.Y)
            Me.clearAndPushPoint()
        End If
    End Sub
    Private Sub clearAndPushPoint()
        Dim lastpos As New Point
        lastpos = two_cell_pos(1)
        two_cell_pos.Clear()
        two_cell_pos.Add(lastpos)
    End Sub
    Public Sub clear()
        two_cell_pos.Clear()
    End Sub
End Class
