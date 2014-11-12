Public Class board
    Public board_width As Integer = 400
    Public board_height As Integer = 400
    Public cells2dArray(board_width, board_height) As cell
    Public genCount As Integer
    Public cellsAliveCount As Integer
    Public Event cellsArrayDimChanged()
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

    Public Sub drawGrid(ByVal e)

        For i = 0 To board_width
            e.Graphics.DrawLine(Pens.Red, 0 + cell.size_unit * i, 0, 0 + cell.size_unit * i, board_height * cell.size_unit)
        Next i

        For i = 0 To board_height
            e.Graphics.DrawLine(Pens.Red, 0, 0 + cell.size_unit * i, board_width * cell.size_unit, 0 + cell.size_unit * i)
        Next i
    End Sub

    Public Sub killAllCells(ByVal limiter)
        'For Each cell In cells2dArray
        '    cell.setCellState(False, limiter)
        '    cell.setNextGenState(False)
        'Next
        For x = 0 To board_width
            For y = 0 To board_height
                cells2dArray(x, y).setCellState(False, limiter)
                cells2dArray(x, y).setNextGenState(False)
            Next
        Next
        limiter.clearBothSets()
    End Sub

    Public Sub redimBoard(ByVal board_width, ByVal board_height, ByVal limiter)
        killAllCells(limiter)
        Me.board_width = board_width
        Me.board_height = board_height
        ReDim cells2dArray(Me.board_width, Me.board_height)
        fillBoard()
        RaiseEvent cellsArrayDimChanged()
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

        increaseGenCount()
    End Sub

    Public Sub countAliveCells(ByVal limiter)
        For Each pos In limiter.cellsPosAndBorder
            If cells2dArray(pos.x, pos.y).getCellState = True Then cellsAliveCount += 1
        Next
    End Sub

    Public Sub increaseGenCount()
        genCount += 1
    End Sub
End Class
