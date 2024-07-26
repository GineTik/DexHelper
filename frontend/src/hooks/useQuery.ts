import { AxiosError, AxiosResponse } from 'axios'
import { useCallback, useEffect, useState } from 'react'

type UseQueryOptions<TData, TError> = {
	query: () => Promise<AxiosResponse<TData, TError>>
	defaultValue?: TData
}

export const useQuery = <TData = any, TError = any>({
	query,
	defaultValue
}: UseQueryOptions<TData, TError>) => {
	const [isLoading, setLoading] = useState(true)
	const [data, setData] = useState<TData | undefined>(defaultValue)
	const [errors, setErrors] = useState<AxiosError[]>()

	const refetch = useCallback(() => {
		query()
			.then((o) => {
				setData(o.data)
				console.log(o.data)
			})
			.catch((errors) => setErrors(errors))
			.finally(() => setLoading(false))
	}, [])

	useEffect(() => {
		refetch()
	}, [])

	return {
		isLoading,
		data,
		setData,
		errors,
		refetch
	}
}
