'use client'

import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  Filler,
} from 'chart.js'
import { Line, Bar } from 'react-chartjs-2'

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
  Filler
)

interface StatsChartProps {
  type: 'line' | 'bar'
  data: {
    labels: string[]
    datasets: {
      label: string
      data: number[]
      borderColor?: string
      backgroundColor?: string
    }[]
  }
  options?: any
}

export function StatsChart({ type, data, options }: StatsChartProps) {
  const defaultOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top' as const,
      },
      title: {
        display: false,
      },
    },
    scales: {
      y: {
        beginAtZero: true,
        grid: {
          color: 'rgba(0, 0, 0, 0.05)',
        },
      },
      x: {
        grid: {
          display: false,
        },
      },
    },
    ...options,
  }

  return (
    <div className="h-[300px] w-full">
      {type === 'line' ? (
        <Line data={data} options={defaultOptions} />
      ) : (
        <Bar data={data} options={defaultOptions} />
      )}
    </div>
  )
}

// Example usage component
export function PlayerPerformanceChart({
  stats,
}: {
  stats: { week: number; yards: number; touchdowns: number }[]
}) {
  const chartData = {
    labels: stats.map((s) => `Week ${s.week}`),
    datasets: [
      {
        label: 'Yards',
        data: stats.map((s) => s.yards),
        borderColor: 'rgb(59, 130, 246)',
        backgroundColor: 'rgba(59, 130, 246, 0.1)',
      },
      {
        label: 'Touchdowns',
        data: stats.map((s) => s.touchdowns * 50), // Scale for visibility
        borderColor: 'rgb(34, 197, 94)',
        backgroundColor: 'rgba(34, 197, 94, 0.1)',
      },
    ],
  }

  return <StatsChart type="line" data={chartData} />
}
