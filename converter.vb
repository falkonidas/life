Imports System.IO
Imports System.Xml
Public Class converter
    Private list_alive As New List(Of Point)

    Public Sub cells_to_list(ByVal board As board)
        For x = 1 To board.board_width - 1
            For y = 1 To board.board_height - 1
                If board.getCellState(x, y) = True Then
                    list_alive.Add(New Point(x, y))
                End If
            Next
        Next
    End Sub

    Public Sub cells_to_board(ByVal board)
        board.clear_board()
        For Each p In list_alive
            board.setCell(p.X, p.Y, True)
        Next
    End Sub

    Public Sub openfromtree(ByVal filename, ByVal board)
        list_alive.Clear()
        Dim myStream As FileStream = File.Open(filename, FileMode.Open)
        Try

            If (myStream IsNot Nothing) Then

                Dim xmldoc As New XmlDataDocument()
                Dim xmlnode As XmlNodeList
                Dim i As Integer

                xmldoc.Load(myStream)
                xmlnode = xmldoc.GetElementsByTagName("size")

                board.board_width = xmlnode(0).ChildNodes.Item(0).InnerText.Trim()
                board.board_height = xmlnode(0).ChildNodes.Item(1).InnerText.Trim()
                board.redim_board(xmlnode(0).ChildNodes.Item(0).InnerText.Trim(), xmlnode(0).ChildNodes.Item(1).InnerText.Trim())
                xmlnode = xmldoc.GetElementsByTagName("cell_pos")
                For i = 0 To xmlnode.Count - 1
                    list_alive.Add(New Point(xmlnode(i).ChildNodes.Item(0).InnerText.Trim(), xmlnode(i).ChildNodes.Item(1).InnerText.Trim()))
                Next
            End If
        Catch Ex As Exception
            MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
        Finally
            ' Check this again, since we need to make sure we didn't throw an exception on open. 
            If (myStream IsNot Nothing) Then
                myStream.Close()
            End If
        End Try

    End Sub
    Public Sub open_xml(ByVal board)

        list_alive.Clear()
        Dim ofd As New OpenFileDialog
        Dim myStream As Stream = Nothing
        With ofd
            .Title = "otwórz plik"
            .Filter = "cellmap (*.cm)|*.cm"
            .InitialDirectory = "c:\"
            .RestoreDirectory = True
        End With

        If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            board.clear_board()
            Try
                myStream = ofd.OpenFile()
                If (myStream IsNot Nothing) Then

                    Dim xmldoc As New XmlDataDocument()
                    Dim xmlnode As XmlNodeList
                    Dim i As Integer

                    xmldoc.Load(myStream)

                    xmlnode = xmldoc.GetElementsByTagName("size")

                    board.board_width = xmlnode(0).ChildNodes.Item(0).InnerText.Trim()
                    board.board_height = xmlnode(0).ChildNodes.Item(1).InnerText.Trim()
                    board.redim_board(xmlnode(0).ChildNodes.Item(0).InnerText.Trim(), xmlnode(0).ChildNodes.Item(1).InnerText.Trim())
                    xmlnode = xmldoc.GetElementsByTagName("cell_pos")
                    For i = 0 To xmlnode.Count - 1
                        list_alive.Add(New Point(xmlnode(i).ChildNodes.Item(0).InnerText.Trim(), xmlnode(i).ChildNodes.Item(1).InnerText.Trim()))
                    Next
                End If

            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
            board.fill_board()
            cells_to_board(board)
        End If
    End Sub

    Public Sub save_to_xml(ByVal board)

        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.Filter = "cellmap (*.cm)|*.cm"
        saveFileDialog1.FilterIndex = 2
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) Then
                Dim writer As New XmlTextWriter(myStream, System.Text.Encoding.UTF8)
                writer.WriteStartDocument(True)
                writer.Formatting = Formatting.Indented
                writer.Indentation = 2
                writer.WriteStartElement("Board")

                createNode("size", board.board_width, board.board_height, writer)

                For Each p In list_alive
                    createNode("cell_pos", p.X, p.Y, writer)
                Next

                writer.WriteEndElement()
                writer.WriteEndDocument()
                writer.Close()

                myStream.Close()
            End If
        End If

        list_alive.Clear()
    End Sub
    Private Sub createNode(ByVal name As String, ByVal x As String, ByVal y As String, ByVal writer As XmlTextWriter)
        writer.WriteStartElement(name)
        writer.WriteStartElement("x")
        writer.WriteString(x)
        writer.WriteEndElement()
        writer.WriteStartElement("y")
        writer.WriteString(y)
        writer.WriteEndElement()
        writer.WriteEndElement()
    End Sub
End Class
