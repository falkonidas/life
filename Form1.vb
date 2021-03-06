﻿Imports System
Imports System.IO
Public Class Form1
    Public Enum click_states
        draw
        move
    End Enum
    Public picbox_click_state As click_states

    Dim path As String = "C:\patterns"

    Dim imagelist As New ImageList

    Public pic_pos As Point
    Public cell_pos As Point
    Public last_cell_pos As Point

    Public m_PanStartPoint As Point

    Public WithEvents board As New board
    'Public oldBoard As New oldBoard
    Public filler As New filler
    Public limiter As New limiter

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        pic_pos = e.Location
        If picbox_click_state = click_states.draw Then
            Try
                last_cell_pos = cell_pos
                cell_pos.X = ((pic_pos.X - cell.size_unit / 2) / cell.size_unit) + 1
                cell_pos.Y = ((pic_pos.Y - cell.size_unit / 2) / cell.size_unit) + 1

                If cell_pos.X < board.board_width And cell_pos.Y < board.board_height Then
                    If e.Button = MouseButtons.Left Then
                        If last_cell_pos <> cell_pos Then
                            filler.get_pos(cell_pos, pic_pos)
                        End If
                        board.cells2dArray(cell_pos.X, cell_pos.Y).setCellState(True, limiter)
                    End If
                    If e.Button = MouseButtons.Right Then
                        board.cells2dArray(cell_pos.X, cell_pos.Y).setCellState(False, limiter)
                    End If
                    'board.countAliveCells() TODO
                End If
            Catch ex As Exception

            End Try
            sender.Invalidate()

        ElseIf picbox_click_state = click_states.move Then

            If e.Button = Windows.Forms.MouseButtons.Left Then

                Dim DeltaX As Integer = (m_PanStartPoint.X - pic_pos.X)
                Dim DeltaY As Integer = (m_PanStartPoint.Y - pic_pos.Y)

                SplitContainer1.Panel2.AutoScrollPosition = _
                New Drawing.Point((DeltaX - SplitContainer1.Panel2.AutoScrollPosition.X), _
                                (DeltaY - SplitContainer1.Panel2.AutoScrollPosition.Y))
            End If
        End If

    End Sub

    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
        If picbox_click_state = click_states.draw Then
            filler.get_pos(cell_pos, pic_pos)
            PictureBox1_MouseMove(sender, e)
        ElseIf picbox_click_state = click_states.move Then
            m_PanStartPoint = New Point(e.X, e.Y)
        End If
    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        filler.clear()
    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        board.drawBoard(e)
        If CheckBox1.Checked Then board.drawGrid(e)
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not System.IO.Directory.Exists(path)) Then
            System.IO.Directory.CreateDirectory(path)
        End If
        ListDirectory(TreeView1, path)

        imagelist.Images.Add(My.Resources.tree_icons.grid)
        imagelist.Images.Add(My.Resources.tree_icons.folder)
        imagelist.Images.Add(My.Resources.tree_icons._error)

        Me.TreeView1.ImageList = imagelist
        paint_controls()

        board.countAliveCells(limiter)
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.Width = board.board_width * cell.size_unit - cell.size_unit
        PictureBox1.Height = board.board_height * cell.size_unit - cell.size_unit
        picbox_click_state = click_states.draw

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        board.nextGen(limiter)
        board.countAliveCells(limiter)
        PictureBox1.Refresh()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If textboxvaliator(TextBox1.Text) = True Then
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            For i = 0 To Int(TextBox1.Text) - 1
                board.nextGen(limiter)
            Next
            board.countAliveCells(limiter)
            PictureBox1.Refresh()
            Windows.Forms.Cursor.Current = Cursors.Default
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        board.killAllCells(limiter)
        'board.count_gen_alive()
        PictureBox1.Refresh()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        'board.board_gen_step()
        'board.count_gen_alive()

        board.nextGen(limiter)
        board.countAliveCells(limiter)

        PictureBox1.Refresh()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If textboxvaliator(TextBox2.Text) = True Then
            Me.Timer1.Interval = TextBox2.Text

            If Timer1.Enabled Then
                Timer1.Stop()
                Button4.Text = "start"
            Else
                Timer1.Start()
                Button4.Text = "stop"
            End If
        End If

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        picbox_click_state = click_states.draw
        paint_controls()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        picbox_click_state = click_states.move
        paint_controls()
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        cell.scaleCells("/")
        control_zoom()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        cell.scaleCells("*")
        control_zoom()
    End Sub

    Private Sub paint_controls()
        If picbox_click_state = click_states.draw Then
            Button5.BackColor = Color.Aqua
            Button6.BackColor = SystemColors.Control
            PictureBox1.Cursor = Cursors.Hand
        ElseIf picbox_click_state = click_states.move Then
            Button6.BackColor = Color.Aqua
            Button5.BackColor = SystemColors.Control
            PictureBox1.Cursor = Cursors.SizeAll
        End If
    End Sub

    Private Sub control_zoom()
        pictureboxResize()
        PictureBox1.Refresh()
        If cell.size_unit = 2 Then
            Button7.Enabled = False
        Else
            Button7.Enabled = True
        End If

        If cell.size_unit = 16 Then
            Button8.Enabled = False
        Else
            Button8.Enabled = True
        End If

    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If textboxvaliator(TextBox4.Text) = True And textboxvaliator(TextBox3.Text) = True Then
            board.redimBoard(TextBox3.Text, TextBox4.Text, limiter)
            PictureBox1.Refresh()
        End If
    End Sub

    Private Sub SplitContainer1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles SplitContainer1.MouseClick
        If Me.SplitContainer1.CanFocus Then
            Button1.Focus()
        End If
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If Me.SplitContainer1.CanFocus Then
            Button1.Focus()
        End If
    End Sub

    Private Sub ListDirectory(ByVal treeView As TreeView, ByVal path As String)
        treeView.Nodes.Clear()
        Dim rootDirectoryInfo = New DirectoryInfo(path)
        treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo))
    End Sub

    Private Shared Function CreateDirectoryNode(ByVal directoryInfo As DirectoryInfo) As TreeNode
        Dim directoryNode = New TreeNode(directoryInfo.Name, 1, 1)

        For Each directory In directoryInfo.GetDirectories()
            directoryNode.Nodes.Add(CreateDirectoryNode(directory))
        Next

        For Each file In directoryInfo.GetFiles()
            If file.Name.Substring(file.Name.Length - 2) = "cm" Then
                directoryNode.Nodes.Add(New TreeNode(file.Name, 0, 0))
            Else
                directoryNode.Nodes.Add(New TreeNode(file.Name, 2, 2))
            End If
        Next

        Return directoryNode
    End Function

    Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        If e.Action = TreeViewAction.ByMouse Then
            If e.Node.Text.Equals("patterns") = False Then
                'board.clear_board()
                Dim TreeNodeName As String = TreeView1.SelectedNode.ToString.Replace("TreeNode: ", String.Empty)
                'converter.openfromtree(path + "\" + TreeNodeName, board)
                'board.fill_board()
                'converter.cells_to_board(board)
                PictureBox1.Refresh()
            End If

        End If

    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        'board.nullGenCount
        'board.count_gen_alive()
    End Sub

    Private Function textboxvaliator(ByVal text)
        Dim answer As Boolean
        If IsNumeric(text) Then
            If Int(text) < 1 Or Int(text) > 1000 Then
                answer = False
            Else
                answer = True
            End If
        Else
            answer = False
        End If
        Return answer
    End Function

    Private Sub pictureboxResize() Handles board.cellsArrayDimChanged
        PictureBox1.Width = board.board_width * cell.size_unit - cell.size_unit
        PictureBox1.Height = board.board_height * cell.size_unit - cell.size_unit
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        PictureBox1.Refresh()
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click

        Dim myStream As Stream
        Dim saveFileDialog1 As New SaveFileDialog()

        saveFileDialog1.Filter = "cellmap (*.cm)|*.cm"
        saveFileDialog1.FilterIndex = 2
        saveFileDialog1.RestoreDirectory = True

        If saveFileDialog1.ShowDialog() = DialogResult.OK Then
            myStream = saveFileDialog1.OpenFile()
            If (myStream IsNot Nothing) Then
                Dim writer As New StreamWriter(myStream)

                writer.Write(board.board_height & Environment.NewLine)
                writer.Write(board.board_width & Environment.NewLine)

                For Each cell In board.cells2dArray
                    writer.Write(Convert.ToInt32(cell.getCellState))
                Next

                writer.Close()
                myStream.Close()
            End If
        End If
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Dim ofd As New OpenFileDialog
        Dim myStream As Stream = Nothing
        With ofd
            .Title = "otwórz plik"
            .Filter = "cellmap (*.cm)|*.cm"
            .InitialDirectory = "c:\patterns"
            .RestoreDirectory = True
        End With

        If ofd.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            board.killAllCells(limiter)
            Try
                myStream = ofd.OpenFile()
                If (myStream IsNot Nothing) Then
                    Dim reader As New StreamReader(myStream)

                    Dim line1 As Integer = reader.ReadLine()
                    Dim line2 As Integer = reader.ReadLine()

                    Dim buffer(line1 * line2) As Char

                    reader.ReadBlock(buffer, 0, line1 * line2)

                    Dim i = 1

                    Try
                        board.redimBoard(line1, line2, limiter)
                        For Each cell In board.cells2dArray
                            cell.setCellState(Convert.ToBoolean(Convert.ToInt32(buffer(i)) - 48), limiter)
                            i += 1
                        Next
                    Catch ex As Exception

                    End Try

                End If

            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()

                End If
            End Try

            'board.fill_board()
            'cells_to_board(board)
            PictureBox1.Refresh()
        End If
    End Sub
End Class
