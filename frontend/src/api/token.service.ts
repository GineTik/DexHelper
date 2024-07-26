import { $api } from './api'

export const TokenEndpoints = {
	GetTokenInformation: (address: string) =>
		`/tokens/get-info?address=${address}`,
	GetTokensPage: (page: number) => `/tokens/last-tokens?page=${page}`
}

export const TokenService = {
	GetTokenInformation: (address: string) => {
		return $api.get(TokenEndpoints.GetTokenInformation(address))
	},
	GetTokensPage: (page: number) => {
		return $api.get(TokenEndpoints.GetTokensPage(page))
	}
}
