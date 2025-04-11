import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { EmailDialogComponent } from './email-dialog/email-dialog.component';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import confetti from 'canvas-confetti';

@Component({
  selector: 'app-emails',
  templateUrl: './emails.component.html',
  styleUrls: ['./emails.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatButtonModule,
    MatDialogModule,
    MatSnackBarModule
  ]
})
export class EmailsComponent {
  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  openEmailDialog(): void {
    const dialogRef = this.dialog.open(EmailDialogComponent, {
      width: '500px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.sendEmail(result);
      }
    });
  }

  private sendEmail(message: string): void {
    const webhookUrl = 'http://localhost:5678/webhook/a93639b3-b5af-4271-8134-cd2a4fb5b51f/email';

    fetch(webhookUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ message })
    }).then(() => {
      this.showConfetti();
      this.snackBar.open('Email bol úspešne odoslaný!', 'Zavrieť', {
        duration: 3000
      });
    }).catch(() => {
      this.snackBar.open('Nastala chyba pri odosielaní emailu', 'Zavrieť', {
        duration: 3000
      });
    });
  }

  private showConfetti(): void {
    confetti({
      particleCount: 100,
      spread: 70,
      origin: { y: 0.6 }
    });
  }
}
