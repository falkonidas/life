Imports System
Imports System.IO
Public Class Form1
    Public Enum click_states
        draw
        move
    End Enum
    Public picbox_click_state As click_states

    Public interpolation As Boolean
    Dim path As String = "C:\patterns"

    Dim imagelist As New ImageList

    Public pic_pos As Point
    Public cell_pos As Point
    Public last_cell_pos As Point

    Public m_PanStartPoint As Point

    Public board As New board
    Public converter As New converter
    Public filler As New filler

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseMove
        pic_pos = e.Location
        If picbox_click_state = click_states.draw Then
            Try
                last_cell_pos = cell_pos
                cell_pos.X = ((pic_pos.X - board.cellsize_unit / 2) / board.cellsize_unit) + 1
                cell_pos.Y = ((pic_pos.Y - board.cellsize_unit / 2) / board.cellsize_unit) + 1
                If cell_pos.X < board.board_width And cell_pos.Y < board.board_height Then
                    If e.Button = MouseButtons.Left Then
                        If last_cell_pos <> cell_pos Then
                            If interpolation = True Then filler.get_pos(cell_pos, pic_pos)
                        End If
                        board.board(cell_pos.X, cell_pos.Y) = True
                    End If

                    If e.Button = MouseButtons.Right Then board.board(cell_pos.X, cell_pos.Y) = False
                    board.count_gen_alive()

                End If
            Catch ex As Exception

            End Try
            PictureBox1.Invalidate()

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
            If interpolation = True Then filler.get_pos(cell_pos, pic_pos)
            PictureBox1_MouseMove(sender, e)
        ElseIf picbox_click_state = click_states.move Then
            m_PanStartPoint = New Point(e.X, e.Y)
        End If
    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseUp
        If interpolation = True Then filler.clear()
    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        board.draw_board(e)
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        board.fill_board()
        interpolation = False

        If (Not System.IO.Directory.Exists(path)) Then
            System.IO.Directory.CreateDirectory(path)
        End If

        ListDirectory(TreeView1, path)
        imagelist.Images.Add(My.Resources.tree_icons.grid)
        imagelist.Images.Add(My.Resources.tree_icons.folder)
        imagelist.Images.Add(My.Resources.tree_icons._error)

        Me.TreeView1.ImageList = imagelist
        paint_controls()
        board.count_gen_alive()
        PictureBox1.Location = New Point(0, 0)
        PictureBox1.Width = board.board_width * board.cellsize_unit - board.cellsize_unit
        PictureBox1.Height = board.board_height * board.cellsize_unit - board.cellsize_unit
        picbox_click_state = click_states.draw
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        board.board_gen_step()
        board.count_gen_alive()
        PictureBox1.Refresh()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If textboxvaliator(TextBox1.Text) = True Then
            Windows.Forms.Cursor.Current = Cursors.WaitCursor
            For i = 0 To Int(TextBox1.Text) - 1
                board.board_gen_step()
            Next
            board.count_gen_alive()
            PictureBox1.Refresh()
            Windows.Forms.Cursor.Current = Cursors.Default
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        board.clear_board()
        board.count_gen_alive()
        PictureBox1.Refresh()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        board.board_gen_step()
        board.count_gen_alive()
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
        zoom_board()
        control_zoom()
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        unzoom_board()
        control_zoom()
    End Sub

    Public Sub paint_controls() ' Handles Me.Load, Button5.Click, Button6.Click
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

    Public Sub control_zoom()
        If board.cellsize_unit = 1 Then
            Button7.Enabled = False
        Else
            Button7.Enabled = True
        End If

        If board.cellsize_unit = 32 Then
            Button8.Enabled = False
        Else
            Button8.Enabled = True
        End If

    End Sub

    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
        converter.cells_to_list(board)
        converter.save_to_xml(board)
        ListDirectory(TreeView1, path)
    End Sub

    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click
        converter.open_xml(board)
        PictureBox1.Refresh()
    End Sub

    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        If textboxvaliator(TextBox4.Text) = True And textboxvaliator(TextBox3.Text) = True Then
            board.redim_board(TextBox4.Text, TextBox3.Text)
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
                board.clear_board()
                Dim TreeNodeName As String = TreeView1.SelectedNode.ToString.Replace("TreeNode: ", String.Empty)
                converter.openfromtree(path + "\" + TreeNodeName, board)
                board.fill_board()
                converter.cells_to_board(board)
                PictureBox1.Refresh()
            End If

        End If

    End Sub

    Private Sub zoom_board()
        board.cellsize_unit /= 2
        board.cell_size = New Size(board.cellsize_unit, board.cellsize_unit)
        PictureBox1.Width = board.board_width * board.cellsize_unit - board.cellsize_unit
        PictureBox1.Height = board.board_height * board.cellsize_unit - board.cellsize_unit
        PictureBox1.Refresh()
    End Sub

    Private Sub unzoom_board()
        board.cellsize_unit *= 2
        board.cell_size = New Size(board.cellsize_unit, board.cellsize_unit)
        PictureBox1.Width = board.board_width * board.cellsize_unit - board.cellsize_unit
        PictureBox1.Height = board.board_height * board.cellsize_unit - board.cellsize_unit
        PictureBox1.Refresh()
    End Sub

    Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
        board.gen_count = 0
        board.count_gen_alive()
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        interpolation = CheckBox1.Checked
    End Sub

    Private Function textboxvaliator(ByVal text)
        Dim answer As Boolean
        If IsNumeric(text) Then
            If Int(text) < 1 Then
                answer = False
            Else
                answer = True
            End If
        Else
            answer = False
        End If
        Return answer
    End Function
End Class
