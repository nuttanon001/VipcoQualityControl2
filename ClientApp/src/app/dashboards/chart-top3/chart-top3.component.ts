import { Component, OnInit, AfterViewInit  } from '@angular/core';
import * as Chart from 'chart.js'

@Component({
  selector: 'app-chart-top3',
  templateUrl: './chart-top3.component.html',
  styleUrls: ['./chart-top3.component.scss']
})
export class ChartTop3Component implements OnInit,AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  canvas: any;
  ctx: any;

  ngAfterViewInit() {
    this.canvas = document.getElementById('myChart');
    this.ctx = this.canvas.getContext('2d');
    let myChart = new Chart(this.ctx, {
      type: 'pie',
      data: {
        labels: ["หน่วยงานสี หุ้มฉนวน และบรรจุภัณฑ์", "FAB กลุ่มวิชิต", "FAB กลุ่มแสน ชอบช้าง"],
        datasets: [{
          label: '# of Require-Qc',
          data: [8, 4, 2],
          backgroundColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)'
          ],
          borderWidth: 1
        }]
      },
      options: {
        responsive: false,
      }
    });
  }
}
