import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ItemsService } from '../../../services/items.service';
import { WarehousesService } from '../../../services/warehouses.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './item-edit.component.html',
  styleUrl: './item-edit.component.css'
})
export class ItemEditComponent {

}
