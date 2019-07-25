public interface IDefaultMenuButtons {
  bool HasButtonsNextPrevious { get; }
  void OnButtonClose();
  void OnButtonBack(System.Action defaultImpl);
  void OnButtonNext();
  void OnButtonPrevious();
}