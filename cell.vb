Public Class cell
    Private pos As Point
    Private alive As Boolean
    Private nextGenAlive As Boolean
    Public Shared size_unit As Integer = 8
    Public Shared cell_size As New Size(size_unit, size_unit)
    'Private Event stateChanged()
    Sub New(ByVal x, ByVal y)
        pos.X = x
        pos.Y = y
    End Sub

    Public Function getCellState()
        Return alive
    End Function

    Public Sub setCellState(ByVal state, ByVal limiter)
        Me.alive = state
        'RaiseEvent stateChanged()
        If state = True Then
            limiter.addCellPosAndBorder(Me.pos.X, Me.pos.Y)
        End If
    End Sub

    Public Sub setNextGenState(ByVal state)
        Me.nextGenAlive = state
        If state = True Then
            'checkCellsPos()
        End If
        'RaiseEvent stateChanged()
    End Sub

    Public Sub drawCell(ByVal e As System.Windows.Forms.PaintEventArgs) ' Handles Me.stateChanged
        If Me.alive = True Then
            'Dim Graphics As System.Drawing.Graphics
            'Graphics = Form1.PictureBox1.CreateGraphics()
            Dim cellLocation As Point = New Point(Me.pos.X * cell_size.Width - size_unit, Me.pos.Y * cell_size.Height - size_unit)

            Dim cell As Rectangle = New Rectangle(cellLocation, cell_size)

            Using cellBrush As SolidBrush = New SolidBrush(Color.FromArgb(0, 0, 0))
                'Graphics.FillRectangle(cellBrush, cell)
                e.Graphics.FillRectangle(cellBrush, cell)
            End Using
        End If

    End Sub

    Public Sub swapStates()
        alive = nextGenAlive
        'setCellState(nextGenAlive, limiter)
    End Sub
End Class
