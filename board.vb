Public Class board
    Public board_width As Integer = 777
    Public board_height As Integer = 777
    Public cells2dArray(board_width, board_height) As cell
    Sub New()
        fillBoard()
    End Sub
    Public Sub fillBoard()
        For x = 0 To board_width
            For y = 0 To board_height
                Me.cells2dArray(x, y) = New cell(x, y)
            Next
        Next
    End Sub
    Public Sub drawBoard(ByVal e)
        For Each cell In cells2dArray
            cell.drawCell(e)
        Next
    End Sub

    Public Sub killAllCells(ByVal limiter)
        For x = 0 To board_width
            For y = 0 To board_height
                cells2dArray(x, y).setCellState(False, limiter)
                cells2dArray(x, y).setNextGenState(False)
            Next
        Next

    End Sub

    Public Sub nextGen(ByVal limiter) 'setCellsNextGenState()

        For Each pos In limiter.cellsPosAndBorder
            Dim counter As Integer = 0
            For Each i In {-1, 1}
                For Each j In {-1, 1}
                    If cells2dArray(pos.x + i, pos.y + j).getCellState = True Then counter += 1
                Next
            Next

            For Each i In {0}
                For Each j In {-1, 1}
                    If cells2dArray(pos.x + i, pos.y + j).getCellState = True Then counter += 1
                Next
            Next

            For Each i In {-1, 1}
                For Each j In {0}
                    If cells2dArray(pos.x + i, pos.y + j).getCellState = True Then counter += 1
                Next
            Next

            If cells2dArray(pos.x, pos.y).getCellState = True Then
                If counter = 2 Or counter = 3 Then
                    cells2dArray(pos.x, pos.y).setNextGenState(True)
                Else
                    cells2dArray(pos.x, pos.y).setNextGenState(False)
                End If

            ElseIf cells2dArray(pos.x, pos.y).getCellState = False Then
                If counter = 3 Then
                    cells2dArray(pos.x, pos.y).setNextGenState(True)
                End If
            End If
        Next

        For Each pos In limiter.cellsPosAndBorder
            cells2dArray(pos.x, pos.y).swapStates()
        Next

        limiter.clearCellsPos()

        For Each pos In limiter.cellsPosAndBorder
            If cells2dArray(pos.x, pos.y).getCellState() = True Then limiter.addCellsPos(pos.x, pos.y)
        Next

        limiter.updateCellsPosAndBorder()
        'gen_count += 1

    End Sub

    Public Sub countAliveCells()

    End Sub
End Class
