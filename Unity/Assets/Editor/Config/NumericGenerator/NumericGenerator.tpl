namespace ET
{
	
    public static class NumericType
    {
	    public const int Max = 10000;

        {{~index=1000~}}
        {{~for numericInfo in list~}}
        public const int {{numericInfo.name}} = {{index}};
        {{~if numericInfo.base~}}
        public const int {{numericInfo.name}}Base = {{numericInfo.name}} * 10 + 1;
        {{~end~}}
        {{~if numericInfo.add~}}
        public const int {{numericInfo.name}}Add = {{numericInfo.name}} * 10 + 2;
        {{~end~}}
        {{~if numericInfo.pct~}}
        public const int {{numericInfo.name}}Pct = {{numericInfo.name}} * 10 + 3;
        {{~end~}}
        {{~if numericInfo.final_add~}}
        public const int {{numericInfo.name}}FinalAdd = {{numericInfo.name}} * 10 + 4;
        {{~end~}}
        {{~if numericInfo.final_pct~}}
        public const int {{numericInfo.name}}FinalPct = {{numericInfo.name}} * 10 + 5;
        {{~end~}}

        {{~index = index+1~}}
        {{~end~}}

    }
}