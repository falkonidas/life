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
    Public Sub get_line() Handles Me.twoPointsReached

        Dim startCellPos = two_cell_pos(0)
        Dim endCellPos = two_cell_pos(1)

        If startCellPos <> endCellPos Then
            Bresenham(startCellPos.X, startCellPos.Y, endCellPos.X, endCellPos.Y, New PlotFunction(AddressOf plot))
            Me.clearAndPushPoint()

        End If
    End Sub
    Public Sub clearAndPushPoint()
        Dim lastpos As New Point
        Try
            lastpos = two_cell_pos(1)
        Catch ex As Exception

        End Try

        two_cell_pos.Clear()
        Try
            two_cell_pos.Add(lastpos)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub clear()
        two_cell_pos.Clear()
    End Sub
End Class
