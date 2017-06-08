<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:my="http://schema.infor.com/InforOAGIS/2">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />   

<xsl:template match="@*|node()">
  <xsl:copy>
    <xsl:apply-templates select="@*|node()"/>
  </xsl:copy>
</xsl:template>


<xsl:template match="@KeyPath[.='yes']">
    <xsl:attribute name="KeyPath">
        <xsl:value-of select="'no'"/>
    </xsl:attribute>
 </xsl:template>

</xsl:stylesheet>