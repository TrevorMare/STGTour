export function ScrollToElement(elementSelector: string): void {
  const element: HTMLElement = document.querySelector(elementSelector);
  if (element !== undefined && element != null) {
    element.scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
  }
}

export function AddClassToElement(elementSelector: string, className: string): void {
  const element: HTMLElement = document.querySelector(elementSelector);
  if (element !== undefined && element != null) {
    element.classList.add(className);
  }
}

export function RemoveClassFromElement(elementSelector: string, className: string): void {
  const element: HTMLElement = document.querySelector(elementSelector);
  if (element !== undefined && element != null) {
    element.classList.remove(className);
  }
} 