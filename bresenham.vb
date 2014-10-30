
Module BresenhamsLineAlgorithm
    Sub Swap(ByRef X As Long, ByRef Y As Long)
        Dim t As Long = X
        X = Y
        Y = t
    End Sub
    Delegate Function PlotFunction(ByVal x As Long, ByVal y As Long) As Boolean
       Sub Bresenham(ByVal x1 As Long, ByVal y1 As Long, ByVal x2 As Long, ByVal y2 As Long, ByVal plot As PlotFunction)
        Dim steep As Boolean = (Math.Abs(y2 - y1) > Math.Abs(x2 - x1))
        If (steep) Then
            Swap(x1, y1)
            Swap(x2, y2)
        End If
        If (x1 > x2) Then
            Swap(x1, x2)
            Swap(y1, y2)
        End If
        Dim deltaX As Long = Math.Abs(x2 - x1)
        Dim deltaY As Long = Math.Abs(y2 - y1)
        Dim err As Long = deltaX / 2
        Dim ystep As Long
        Dim y As Long = y1
        If (y1 < y2) Then
            ystep = 1
        Else
            ystep = -1
        End If
        For x As Long = x1 To x2
            Dim result As Boolean
            If (steep) Then result = plot(y, x) Else result = plot(x, y)
            If (Not result) Then Exit Sub
            err = err - deltaY
            If (err < 0) Then
                y = y + ystep
                err = err + deltaX
            End If
        Next
    End Sub
    Function plot(ByVal x As Long, ByVal y As Long) As Boolean
        Form1.board.board(Int(x), Int(y)) = True
        Return True
    End Function
End Module
