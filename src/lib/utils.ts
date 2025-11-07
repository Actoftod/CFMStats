import { type ClassValue, clsx } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function formatDate(date: Date | string): string {
  return new Date(date).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  })
}

export function formatDateTime(date: Date | string): string {
  return new Date(date).toLocaleString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: '2-digit',
  })
}

export function formatNumber(num: number): string {
  return new Intl.NumberFormat('en-US').format(num)
}

export function formatPercentage(num: number, decimals: number = 1): string {
  return `${(num * 100).toFixed(decimals)}%`
}

export function calculateCompletionPercentage(completions: number, attempts: number): number {
  if (attempts === 0) return 0
  return (completions / attempts) * 100
}

export function calculatePasserRating(
  completions: number,
  attempts: number,
  yards: number,
  touchdowns: number,
  interceptions: number
): number {
  if (attempts === 0) return 0

  const a = Math.min(Math.max(((completions / attempts - 0.3) * 5), 0), 2.375)
  const b = Math.min(Math.max(((yards / attempts - 3) * 0.25), 0), 2.375)
  const c = Math.min(Math.max((touchdowns / attempts * 20), 0), 2.375)
  const d = Math.min(Math.max((2.375 - (interceptions / attempts * 25)), 0), 2.375)

  return ((a + b + c + d) / 6) * 100
}
