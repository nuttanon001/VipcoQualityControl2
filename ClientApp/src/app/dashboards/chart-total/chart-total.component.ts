import { Component, OnInit, AfterViewInit } from '@angular/core';
import * as Chart from 'chart.js'

@Component({
  selector: 'app-chart-total',
  templateUrl: './chart-total.component.html',
  styleUrls: ['./chart-total.component.scss']
})
export class ChartTotalComponent implements OnInit, AfterViewInit {

  constructor() { }

  ngOnInit() {
  }

  canvas: any;
  ctx: any;

  ngAfterViewInit() {
    console.log("Load");
    this.canvas = document.getElementById('myChart1');
    this.ctx = this.canvas.getContext('2d');
    let myChart = new Chart(this.ctx, {
      type: 'pie',
      data: {
        labels: ["Total"],
        datasets: [{
          label: '# of Require-Qc',
          data: [15],
          backgroundColor: [
            'rgba(255, 99, 132, 1)',
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
