Public Class cell
    Private pos As Point
    Private alive As Boolean
    Private nextGenAlive As Boolean
    Private isBorder As Boolean
    Public Shared size_unit As Integer = 8
    Public Shared cell_size As New Size(size_unit, size_unit)
    Sub New(ByVal x, ByVal y)
        pos.X = x
        pos.Y = y
    End Sub

    Public Shared Sub scaleCells(ByVal action)
        If action = "/" Then cell.size_unit /= 2
        If action = "*" Then cell.size_unit *= 2
        cell.cell_size = New Size(cell.size_unit, cell.size_unit)
    End Sub

    Public Function getCellState()
        Return alive
    End Function

    Public Sub setCellState(ByVal state, ByVal limiter)
        If isBorder = False Then
            Me.alive = state
            If state = True Then
                limiter.addCellPosAndBorder(Me.pos.X, Me.pos.Y)
            End If
        End If
    End Sub

    Public Sub setNextGenState(ByVal state)
        Me.nextGenAlive = state
    End Sub

    Public Sub drawCell(ByVal e As System.Windows.Forms.PaintEventArgs)
        If Me.alive = True Then
            Dim cellLocation As Point = New Point(Me.pos.X * cell_size.Width - size_unit, Me.pos.Y * cell_size.Height - size_unit)

            Dim cell As Rectangle = New Rectangle(cellLocation, cell_size)

            Using cellBrush As SolidBrush = New SolidBrush(Color.FromArgb(0, 0, 0))
                e.Graphics.FillRectangle(cellBrush, cell)
            End Using
        End If

    End Sub

    Public Sub swapStates()
        alive = nextGenAlive
    End Sub
End Class
