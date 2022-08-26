import { ApiResponse } from "../models/api-Response"

export interface LineGraphData {
  totalBalance: number
  labels: string[]
  figures: number[]
}

export interface LineGraphDataResponse extends ApiResponse {
  result: LineGraphData
}