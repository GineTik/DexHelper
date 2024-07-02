import { TooltipProvider } from "@radix-ui/react-tooltip"

const Providers = ({children}: any) => {
  return (
    <>
        <TooltipProvider>
            {children}
        </TooltipProvider>
    </>
  )
}

export default Providers
