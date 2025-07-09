import { NgClass } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink,NgClass],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected title = 'AdvocateAssist';
isSidebarCollapsed = false;

  toggleSidebar(): void {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
}
}
