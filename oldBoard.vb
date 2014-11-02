Public Class oldBoard
    Public cellsize_unit As Integer = 8
    Public cell_size As New Size(cellsize_unit, cellsize_unit)
    Public board_width As Integer = 300
    Public board_height As Integer = 150
    Private board(board_width, board_height) As Boolean
    Private nextgen_board(board_width, board_height)
    Private gen_count As Integer

    Public Sub redim_board(ByVal board_width, ByVal board_height)
        Me.board_width = board_width
        Me.board_height = board_height
        ReDim Me.board(board_width, board_height)
        ReDim Me.nextgen_board(board_width, board_height)
        clear_board()
        fill_board()
        Form1.PictureBox1.Width = board_width * cellsize_unit - cellsize_unit
        Form1.PictureBox1.Height = board_height * cellsize_unit - cellsize_unit
    End Sub
    Public Sub fill_board()
        For x = 0 To board_width
            For y = 0 To board_height
                Me.board(x, y) = New Boolean
                Me.nextgen_board(x, y) = New Boolean
            Next
        Next
    End Sub

    Public Sub clear_board()
        For x = 0 To board_width
            For y = 0 To board_height
                Me.board(x, y) = False
                Me.nextgen_board(x, y) = False
            Next
        Next

        count_gen_alive()
        Form1.PictureBox1.Refresh()
    End Sub
    Public Sub setCell(ByVal x, ByVal y, ByVal state)
        Me.board(x, y) = state
    End Sub

    Public Function getCellState(ByVal x, ByVal y)
        Return board(x, y)
    End Function

    Public Sub setCellsFromList(ByVal list)
        For Each cell In list
            Me.board(cell.x, cell.y) = True
        Next
    End Sub

    Public Sub draw_board(ByVal e As System.Windows.Forms.PaintEventArgs)

        For x = 0 To board_width - 1
            For y = 0 To board_height - 1
                Dim cellLocation As Point = New Point(x * cell_size.Width - cellsize_unit, y * cell_size.Height - cellsize_unit)

                Dim cell As Rectangle = New Rectangle(cellLocation, cell_size)

                Using cellBrush As SolidBrush = If(board(x, y) = True, New SolidBrush(Color.FromArgb(0, 0, 0)), New SolidBrush(Color.FromArgb(255, 255, 255)))
                    e.Graphics.FillRectangle(cellBrush, cell)
                End Using
            Next
        Next

    End Sub
    Public Sub board_gen_step()
        For x = 1 To board_width - 1
            For y = 1 To board_height - 1
                Dim counter As Integer = 0
                For Each i In {-1, 1}
                    For Each j In {-1, 1}
                        If board(x + i, y + j) = True Then counter += 1
                    Next
                Next

                For Each i In {0}
                    For Each j In {-1, 1}
                        If board(x + i, y + j) = True Then counter += 1
                    Next
                Next

                For Each i In {-1, 1}
                    For Each j In {0}
                        If board(x + i, y + j) = True Then counter += 1
                    Next
                Next

                If board(x, y) = True Then
                    If counter = 2 Or counter = 3 Then
                        nextgen_board(x, y) = True
                    Else
                        nextgen_board(x, y) = False
                    End If

                ElseIf board(x, y) = False Then
                    If counter = 3 Then
                        nextgen_board(x, y) = True
                    End If
                End If
            Next
        Next

        For x = 1 To board_width - 1
            For y = 1 To board_height - 1
                board(x, y) = nextgen_board(x, y)
            Next
        Next

        gen_count += 1

    End Sub

    Public Sub count_gen_alive()
        Dim counter As Integer
        For x = 1 To board_width - 1
            For y = 1 To board_height - 1
                If board(x, y) = True Then counter += 1
            Next
        Next
        Form1.Label3.Text = "alive: " & counter
        Form1.Label2.Text = "generation: " & gen_count
    End Sub

    Public Sub nullGenCount()
        gen_count = 0
    End Sub
End Class
