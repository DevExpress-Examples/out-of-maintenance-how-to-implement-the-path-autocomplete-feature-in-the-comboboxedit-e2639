Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports DevExpress.XtraEditors
Imports DevExpress.Utils.Win
Imports DevExpress.XtraEditors.Popup

Namespace WindowsApplication1
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			Dim TempAutoCompleteFilesHelper As AutoCompleteFilesHelper = New AutoCompleteFilesHelper(comboBoxEdit1)
		End Sub
	End Class

	Public Class DropDownListHelper

	  Public Shared Function GetDropDownList(ByVal path As String) As List(Of String)
		  Dim result As New List(Of String)()
		  path = CheckPath(path)
		  If (Not Directory.Exists(path)) Then
			  Return result
		  End If
		  result.AddRange(GetFiles(path))
		  result.AddRange(GetFolders(path))
		  Return result
	  End Function
		Public Shared Function CheckPath(ByVal filePath As String) As String
			If filePath = String.Empty Then
				Return filePath
			End If
            Dim result As String = Path.GetDirectoryName(filePath)
			If Directory.Exists(result) Then
				Return result
			End If
            result = filePath & Path.VolumeSeparatorChar + Path.DirectorySeparatorChar
			If Directory.Exists(result) Then
				Return result
			End If
			Return filePath
		End Function

		Private Shared Function GetFiles(ByVal path As String) As String()
			Dim result() As String = { }
			Try

				result = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
			Catch ex As Exception

			End Try
			Return result
		End Function

		Private Shared Function GetFolders(ByVal path As String) As String()
			Dim result() As String = { }

			Try

				result = Directory.GetDirectories(path, "*.*", SearchOption.TopDirectoryOnly)
			Catch ex As Exception

			End Try
			Return result
		End Function
	End Class

	Public Class AutoCompleteFilesHelper

		Private _Editor As ComboBoxEdit
		Public Sub New(ByVal editor As ComboBoxEdit)
			_Editor = editor
			AddHandler _Editor.EditValueChanged, AddressOf _Editor_EditValueChanged
			_Editor.Properties.ImmediatePopup = True
			AddHandler _Editor.KeyDown, AddressOf _Editor_KeyDown
		End Sub

		Private Sub _Editor_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
			Dim editor As TextEdit = TryCast(sender, TextEdit)
			If e.KeyCode = Keys.End Then
				editor.SelectionStart = editor.Text.Length
				e.Handled = True
			End If

		End Sub

		Private Sub _Editor_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
			UpdateDataSource()
		End Sub

		Private Sub UpdateDataSource()
			Dim needOpen As Boolean = _Editor.IsPopupOpen
			RepopulateList()
			If needOpen Then
				_Editor.ShowPopup()
			End If
		End Sub
		Private Sub RepopulateList()
			_Editor.Properties.Items.Clear()
			_Editor.Properties.Items.AddRange(DropDownListHelper.GetDropDownList(_Editor.Text))
		End Sub
	End Class
End Namespace